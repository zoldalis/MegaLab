// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//function InpOnChange() {


//}

//HTMLInputElementObject.addEventListener('input', function (evt) {
//    something(this.value);
//});

/* event listener */
document.getElementsByName("typeinp")[0].addEventListener('change', doThing);

/* function */
function doThing() {
    alert('Horray! Someone wrote "' + this.value + '"!');
}