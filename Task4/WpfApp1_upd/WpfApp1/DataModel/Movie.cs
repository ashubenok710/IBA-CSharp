﻿using GalaSoft.MvvmLight;
using System.ComponentModel.DataAnnotations;

namespace WpfApp1.Model
{
    public partial class Movie : ObservableObject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ProductionDate { get; set; } = null!;
        public string Raiting { get; set; } = null!;
        public int DirectorId { get; set; }

        public Person Director;
    }
}
