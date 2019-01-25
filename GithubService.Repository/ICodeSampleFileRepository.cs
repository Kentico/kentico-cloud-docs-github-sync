﻿using GithubService.Models.CodeSamples;
using System.Threading.Tasks;

namespace GithubService.Repository
{
    public interface ICodeSampleFileRepository
    {
        Task<CodeSampleFile> GetAsync(string filePath);

        Task<CodeSampleFile> StoreAsync(CodeSampleFile file);

        Task ArchiveAsync(CodeSampleFile file);
    }
}