﻿namespace NHibernateCourse.QuickStart.Model
{
    public class Penalty
    {
        public virtual object AttachedTo { get; set; }
        public virtual int Score { get; set; }
    }
}