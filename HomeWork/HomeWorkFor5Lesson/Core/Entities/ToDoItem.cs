namespace HomeWorkFor5Lesson.Core.Entities
{
    internal class ToDoItem
    {
        public Guid Id { get; }
        //public User User;
        public ToDoUser User { get; } // Мне кажется, этот тип использовать здесь логичнее
        public string Name { get; }
        public DateTime CreatedAt { get; }
        public ToDoItemState State { get; set; }
        public DateTime? StateChangedAt { get; set; }

        public ToDoItem(ToDoUser user, string name)
        {
            Id = Guid.NewGuid();
            User = user;
            Name = name;
            State = ToDoItemState.Active;
            CreatedAt = DateTime.Now;
        }

    }
}
