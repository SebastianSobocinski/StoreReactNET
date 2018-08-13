let _categoriesOpen = false;

export class StoreVisuals
{
    static getCategoriesOpen()
    {
        return _categoriesOpen;
    }
    static setCategoriesOpen(value)
    {
        _categoriesOpen = value;
    }

    static animateCategories(from, step, to)
    {
        if (from < to)
        {
            if (step < to)
            {
                step += 3
                document.getElementById('sideCategoryList').style.height = step + "px";
                requestAnimationFrame(() => this.animateCategories(from, step, to));
            }
        }
        else
        {
            if (step > to)
            {
                step -= 3
                document.getElementById('sideCategoryList').style.height = step + "px";
                requestAnimationFrame(() => this.animateCategories(from, step, to));
            }

        }

    }
}

