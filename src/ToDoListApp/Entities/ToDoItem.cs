using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Utils;

namespace ToDoListApp.Entities
{
    /// <summary>
    /// ToDoItem Entity model
    /// </summary>
    public class ToDoItem
    {

        public long ID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateLastEdited { get; set; }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnIsCompletedChanged(); UpdateLastEditDate(); }
        }
        private string _text;
        public string Text
        {
            get => _text;
            set { _text = value; UpdateLastEditDate(); }

        }

        private Priority _priority;
        public Priority Priority
        {
            get => _priority;
            set { _priority = value; UpdateLastEditDate(); }
        }

        public ToDoItem()
        {
            DateTime now = DateTime.Now;
            ID = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            DateCreated = now;
            DateLastEdited = null;
            DateCompleted = null;
            _isCompleted = false;
            _text = "";
            _priority = Priority.LOW;
        }

        private void UpdateLastEditDate()
        {
            DateLastEdited = DateTime.Now;
            TraceUtil.LogMethodCall();
        }

        private void OnIsCompletedChanged()
        {
            if (IsCompleted)
            {
                DateCompleted = DateTime.Now;
            }
            else
            {
                DateCompleted = null;
            }

            TraceUtil.LogMethodCall();
        }
    }
}
