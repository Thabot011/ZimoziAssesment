﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Product
{
    public class AllProductsDto
    {
        public int TotalCount { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
