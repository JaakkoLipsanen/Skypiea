using System.Collections.Generic;

namespace Flai.DataStructures
{
    public interface IUndoRedo
    {
        void Undo();
        void Redo();
    }

    public interface IUndoRedoStack<T>
    {
        bool CanUndo { get; }
        bool CanRedo { get; }

        void Push(T value);
        void Undo();
        void Redo();
    }

    public class UndoRedoStack<T> : IUndoRedoStack<T>
        where T : IUndoRedo
    {
        private readonly Stack<T> _undoStack = new Stack<T>();
        private readonly Stack<T> _redoStack = new Stack<T>();

        public int UndoCount
        {
            get { return _undoStack.Count; }
        }

        public int RedoCount
        {
            get { return _redoStack.Count; }
        }

        public virtual bool CanUndo
        {
            get { return this.UndoCount != 0; }
        }

        public virtual bool CanRedo
        {
            get { return this.RedoCount != 0; }
        }

        public void Push(T value)
        {
            _redoStack.Clear();
            _undoStack.Push(value);

            this.OnPush(value);
        }

        public void Undo()
        {
            if (this.CanUndo)
            {
                T value = _undoStack.Pop();
                _redoStack.Push(value);

                value.Undo();
                this.OnUndo(value);
            }
        }

        public void Redo()
        {
            if (this.CanRedo)
            {
                T value = _redoStack.Pop();
                _undoStack.Push(value);

                value.Redo();
                this.OnRedo(value);
            }
        }

        protected virtual void OnPush(T value) { }
        protected virtual void OnUndo(T value) { }
        protected virtual void OnRedo(T value) { }
    }
}
