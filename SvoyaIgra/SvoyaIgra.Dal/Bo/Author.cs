﻿namespace SvoyaIgra.Dal.Bo;

public class Author
{
    public int  Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Topic> Topics { get; set; }
}