// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
const uriC = 'api/Cats';
const uriS = 'api/Speciess';
let cats = [];
let species = [];


///// список котов
function getCats() {
    fetch(uriC)
        .then(response => response.json())
        .then(data => _displayCats(data))
        .catch(error => console.error('Unable to get cats.', error));
}
function _displayCats(data) {
    const tBody = document.getElementById('cats');
    tBody.innerHTML = '';
    data.forEach(cat => {

        var text = `<div class="divform"><dl><dt class="name">${cat.name}</dt>
                    <img src="img/${cat.img}" class="img1" >
                    <div><a class="alltext">Длительность:   ${cat.age}</a><br/>
                    <a class="alltext">Порода:   `;

        cat.species(specie => {
            text += specie;
        });
        text = text.slice(0, -2);
        text += '</a><br/>';

        text += `</a><br/>
                  <a class="alltext">Год премьеры:   ${cat.info}</a><br/>           </div><dl></div>`;
        tBody.innerHTML += text;


    });
    cats = data;
}

function getSpecieForCats() {
    var id = document.getElementById('selectspecie').value;
    fetch(uriS + `?id=${id}&shoto=darova`)
        .then(response => response.json())
        .then(data => _displaySpecieForCats(data))
        .catch(error => console.error('Unable to get species.', error));
}
function _displaySpecieForCats(data) {
    const a = document.getElementById("selectspecie");
    //a +=`<a class="alltext">Genre:   </a>`
    data.forEach(specie => {
        a += `  ${specie.name} `;
    });
    a += `</br>`;

    species = data;
}

















// Write your JavaScript code.
$(document).ready(function () {
    $("body").css("opacity", "1");
}); //Get the button:
mybutton = document.getElementById("myBtn");

$(document).ready(function () {
	$(window).scroll(function () {
		if ($(this).scrollTop() > 50) {
			$('#back-to-top').fadeIn();
		} else {
			$('#back-to-top').fadeOut();
		}
	});
	// scroll body to 0px on click
	$('#back-to-top').click(function () {
		$('body,html').animate({
			scrollTop: 0
		}, 400);
		return false;
	});
});