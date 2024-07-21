using System;
using System.Collections.Generic;

namespace Movies.Data.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public string ReleaseYear { get; set; } = null!;
}
