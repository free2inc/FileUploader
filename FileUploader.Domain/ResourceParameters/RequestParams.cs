﻿using System.ComponentModel.DataAnnotations;

namespace FileUploader.Domain.ResourceParameters
{
    public class RequestParams
    {
        const int maxPageSize = 10;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize; 
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value; 
        }
        public string? FileExtension { get; set; }

    }
}
