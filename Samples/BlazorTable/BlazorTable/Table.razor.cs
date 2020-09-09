using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTable
{
    public class TableBase<TItem> : ComponentBase
    {
        private const int pagerSize = 6;

        public int PagesCount { get; set; }
        public int CurPage { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        [Parameter]
        public RenderFragment TableHeader { get; set; }
        [Parameter]
        public RenderFragment<TItem> TableRow { get; set; }
        [Parameter]
        public IEnumerable<TItem> Items { get; set; }
        [Parameter]
        public int PageSize { get; set; }
        public IEnumerable<TItem> ItemList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurPage = 1;
            ItemList = Items.Skip((CurPage - 1) * PageSize).Take(PageSize);
            PagesCount = (int)Math.Ceiling(Items.Count() / (decimal)PageSize);
            SetPagerSize(PaginationAction.Forward);
        }
        public void UpdateList(int currentPage)
        {
            ItemList = Items.Skip((currentPage - 1) * PageSize).Take(PageSize);
            CurPage = currentPage;
            StateHasChanged();
        }
        public void SetPagerSize(PaginationAction direction)
        {
            if (direction == PaginationAction.Forward && EndPage < PagesCount)
            {
                StartPage = EndPage + 1;
                if (EndPage + pagerSize < PagesCount)
                {
                    EndPage = StartPage + pagerSize - 1;
                }
                else
                {
                    EndPage = PagesCount;
                }
                StateHasChanged();
            }
            else if (direction == PaginationAction.Back && StartPage > 1)
            {
                EndPage = StartPage - 1;
                StartPage = StartPage - pagerSize;
            }
            CurPage = StartPage;
            UpdateList(CurPage);
        }
        public void NavigateToPage(PaginationAction direction)
        {
            if (direction == PaginationAction.Next)
            {
                if (CurPage < PagesCount)
                {
                    if (CurPage == EndPage)
                    {
                        SetPagerSize(PaginationAction.Forward);
                    }
                    CurPage += 1;
                }
            }
            else if (direction == PaginationAction.Previous)
            {
                if (CurPage > 1)
                {
                    if (CurPage == StartPage)
                    {
                        SetPagerSize(PaginationAction.Back);
                    }
                    CurPage -= 1;
                }
            }
            UpdateList(CurPage);
        }
    }

    public enum PaginationAction
    {
        Forward,
        Back,
        Next,
        Previous
    }
}
