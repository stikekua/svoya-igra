﻿using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.Dal.Services;

public class ThemeService<TContext> : IThemeService where TContext : DbContext
{
    private readonly TContext _dbContext;

    public ThemeService(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ThemeDto>> GetAllThemesAsync()
    {
        var themes = _dbContext.Set<Theme>()
            .AsNoTracking()
            .ToList();

        return themes.Select(t => t.ToDto());
    }

    public async Task<ThemeDto?> GetThemeAsync(int id)
    {
        var theme = await _dbContext.Set<Theme>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        return theme?.ToDto();
    }

    public async Task<IEnumerable<QuestionDto>?> GetThemeQuestionsAsync(int themeId)
    {
        var questions = _dbContext.Set<Question>()
            .Where(q => q.ThemeId == themeId)
            .AsNoTracking();

        return questions.Select(q => q.ToDto());
    }
    public async Task<ThemeDto?> CreateThemeAsync(string name, ThemeDifficulty difficulty)
    {
        var theme = new Theme
        {
            Name = name,
            Difficulty = difficulty
        };
        _dbContext.Set<Theme>().Add(theme);
        await _dbContext.SaveChangesAsync();
        return theme.ToDto();
    }
    
    public Task<ThemeDto?> UpdateThemeAsync(int id, string name, ThemeDifficulty difficulty)
    {
        throw new NotImplementedException();
    }
    public Task<ThemeDto?> DeleteThemeAsync(int id)
    {
        throw new NotImplementedException();
    }
}