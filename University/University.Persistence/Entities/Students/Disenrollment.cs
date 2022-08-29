﻿namespace University.Persistence.Entities.Students;
public class Disenrollment : EntityBase
{
    public virtual Student Student { get; set; }
    public virtual Course Course { get; set; }
    public virtual DateTime DateTime { get; set; }
    public virtual string Comment { get; set; }
}

