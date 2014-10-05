
if (!window.requestAnimationFrame) {
	window.requestAnimationFrame = 
		(window.webkitRequestAnimationFrame ||
		window.mozRequestAnimationFrame ||
		window.oRequestAnimationFrame ||
		window.msRequestAnimationFrame ||
		function(callback) {
			return window.setTimeout(callback, 1000 / 60);
		});
}


//// UTILS COLLECTION

var utils = {};

utils.captureMouse = function(element) {
	var mouse = { x : 0, y : 0 };
	
	element.addEventListener("mousemove", function(event) {
		var x, y;
		
		if (event.pageX || event.pageY) {
			x = event.pageX;
			y = event.pageY;
		} else {
			x = event.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
			y = event.clientY + document.body.scrollTop + document.documentElement.scrollTop;
		}
				
		mouse.x = x - element.offsetLeft;
		mouse.y = y - element.offsetTop;
		}, false);
		
	return mouse;
};

utils.toRadians = function(degrees) {
	return degrees * Math.PI / 180;
};

utils.toDegrees = function(radians) {
	return radians * 180 / Math.PI;
};

utils.itoa = function(int) {
	return String.fromCharCode(int);
};

// returns a random integer between min and max (inclusive)
utils.getRandomInt = function(min, max) {
	return Math.floor( Math.random() * (max - min + 1) ) + min;
}

utils.getRandomBool = function(){
	if(utils.getRandomInt(1,2) % 2 != 1){
		return true;
	}
	return false;
}

utils.getRandomFloat = function(min, max) {
	return Math.random() * (max - min) + min;
}

utils.getRandomScreenXorY = function(screenDimension){
	return Math.random() * screenDimension;
}