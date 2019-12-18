// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const apiBase = 'https://localhost:44355/api/';
const productsResourceLocation = 'products';
const categoriesResourceLocation = 'categories';


async function  getProducts(count) {
    var url = new URL(apiBase + productsResourceLocation);
    if (count !== undefined) {
        url.searchParams.append('count', count);
    }

    var response = await fetch(url);
    var result = await response.json();

    var template = '<p>#key : #value<p>';
    result.data.forEach(p => {
        var keys = Object.keys(p);
        keys.forEach(k => {
            document.getElementById(productsResourceLocation).innerHTML += template.replace('#key', k).replace('#value',p[k]);
        });
    });
}

async function getCategories(count) {
    var url = new URL(apiBase + categoriesResourceLocation);

    if (count !== undefined) {
        url.searchParams.append('count', count);
    }

    var response = await fetch(url);
    var result = await response.json();

    var template = '<p>#key : #value<p>';
    result.data.forEach(c => {
        var keys = Object.keys(c);
        keys.forEach(k => {
            document.getElementById(categoriesResourceLocation).innerHTML += template.replace('#key', k).replace('#value', c[k]);
        });
    });
}
