using System;
using System.Collections.Generic;

namespace MoviesREST.Models
{
    public partial class Movie
    {
        public int FilmId { get; set; }
        public string FilmName { get; set; }
        public string DirectorName { get; set; }
        public string CountryName { get; set; }
        public string Language { get; set; }
        public string Certificate { get; set; }
        public string StudioName { get; set; }
        public DateTime? FilmReleaseDate { get; set; }
        public string FilmSynopsis { get; set; }
        public int? FilmRunTimeMinutes { get; set; }
        public int? FilmBudgetDollars { get; set; }
        public int? FilmBoxOfficeDollars { get; set; }
        public int? FilmOscarNominations { get; set; }
        public int? FilmOscarWins { get; set; }
    }
}
