
// JavaScript for smooth transitions
document.body.addEventListener('mouseover', () => {
    document.body.style.background = 'url("/images/surf.jpg") no-repeat center center fixed';
    document.body.style.backgroundSize = 'cover';
});

document.body.addEventListener('mouseout', () => {
    document.body.style.background = 'url("/images/surf.jpg") no-repeat center center fixed';
    document.body.style.backgroundSize = 'cover';
});

function showimage(){
    var img = document.createElement("img");
    img.src = "https://footballstadiumdigest.com/wp-content/uploads/2017/01/USF-Bulls.png"
    document.getElementById("image").appendChild(img);
};

