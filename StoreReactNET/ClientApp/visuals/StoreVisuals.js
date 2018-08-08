import $ from 'jquery';

let categoriesOpen = false;

document.addEventListener("DOMContentLoaded", () =>
{
    document.getElementById("sideCategoryTitle").addEventListener("click", () =>
    {
        let categoryList = document.getElementById("sideCategoryList");
        if (categoriesOpen)
        {
            let startSize = categoryList.clientHeight;
            requestAnimationFrame(() => animateCategories(startSize, startSize, 0))
            categoriesOpen = false;
        }
        else
        {
            let desiredSize = categoryList.scrollHeight;
            categoriesOpen = true;
            requestAnimationFrame(() => animateCategories(0, 0, desiredSize))
        }
    })
   
    
})

function animateCategories(from, step, to)
{
    if (from < to)
    {
        if (step < to)
        {
            step += 3
            document.getElementById('sideCategoryList').style.height = step + "px";
            requestAnimationFrame(() => animateCategories(from, step, to));
        }
    }
    else
    {
        if (step > to)
        {
            step -= 3
            document.getElementById('sideCategoryList').style.height = step + "px";
            requestAnimationFrame(() => animateCategories(from, step, to));
        }
       
    }
    
}