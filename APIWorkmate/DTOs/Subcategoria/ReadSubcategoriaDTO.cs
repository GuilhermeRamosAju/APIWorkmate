﻿namespace APIWorkmate.DTOs.Subcategoria;

public class ReadSubcategoriaDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CategoriaNome { get; set; } = string.Empty;
}
