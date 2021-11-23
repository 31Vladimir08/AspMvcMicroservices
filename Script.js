// JavaScript source code
ShowAllProducts();
ShowAllCategories();

function ShowAllProducts() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:44359/api/ProductApi", true);
    xhttp.send();

    xhttp.onreadystatechange = function () {
        var tbody = document.getElementById("apiTableProducts").querySelector("tbody");
        tbody.innerHTML = "";
        if (this.readyState == 4 && this.status == 200) {
            JSON.parse(this.responseText).forEach(function (data, index) {
                tbody.innerHTML += "<tr><td>" + data.productID + "</td>" + "<td>" + data.productName + "</td></tr>";
            });
        }
    };
}

function ShowAllCategories() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:44359/api/CategoryApi", true);
    xhttp.send();

    xhttp.onreadystatechange = function () {
        var tbody = document.getElementById("apiTableCategories").querySelector("tbody");
        tbody.innerHTML = "";
        if (this.readyState == 4 && this.status == 200) {
            JSON.parse(this.responseText).forEach(function (data, index) {
                tbody.innerHTML += "<tr><td>" + data.categoryID + "</td>" + "<td>" + data.categoryName + "</td></tr>";
            });
        }
    };
}