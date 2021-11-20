namespace MealPlanner.CoreLibrary.Models;
public class NoteModel : IMappable
{
    public int ID { get; set; }
    public string Breakfast { get; set; } = "";
    public string Lunch { get; set; } = "";
    public string Dinner { get; set; } = "";
    public DateOnly WhatDate { get; set; }
    public string Snacks { get; set; } = "";
    public string Desserts { get; set; } = "";
}