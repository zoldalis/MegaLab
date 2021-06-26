
let type = "";

/* event listener */
document.getElementById("typeinp").addEventListener('change', doThing);

/* function */
function doThing() {
    type = this.value;

    switch (type) {
        case "Датчик Температуры":
            {
                document.getElementById("temp").style.display = "block";
                document.getElementById("press").style.display = "none";
                document.getElementById("move").style.display = "none";
                document.getElementById("light").style.display = "none";
                document.getElementById("humid").style.display = "none";
            }
            break;
        case "Датчик Давления":
            {
                document.getElementById("temp").style.display = "none";
                document.getElementById("press").style.display = "block";
                document.getElementById("move").style.display = "none";
                document.getElementById("light").style.display = "none";
                document.getElementById("humid").style.display = "none";
            }
            break;
        case "Датчик Движения":
            {
                document.getElementById("temp").style.display = "none";
                document.getElementById("press").style.display = "none";
                document.getElementById("move").style.display = "block";
                document.getElementById("light").style.display = "none";
                document.getElementById("humid").style.display = "none";
            }
            break;
        case "Датчик Освещения":
            {
                document.getElementById("temp").style.display = "none";
                document.getElementById("press").style.display = "none";
                document.getElementById("move").style.display = "none";
                document.getElementById("light").style.display = "block";
                document.getElementById("humid").style.display = "none";
            }
            break;
        case "Датчик Влажности":
            {
                document.getElementById("temp").style.display = "none";
                document.getElementById("press").style.display = "none";
                document.getElementById("move").style.display = "none";
                document.getElementById("light").style.display = "none";
                document.getElementById("humid").style.display = "block";
            }
            break;
        default:
    }



}


