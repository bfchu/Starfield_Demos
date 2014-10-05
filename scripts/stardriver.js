

var display = document.getElementById("gameCanvas"); //capture the canvas element.
var ctx = display.getContext('2d'); //get the 2D drawing context.
window.onresize = fitDisplayToWindow;  //make the window call fitDisplayToWindow() when the user resizes it.


//// GLOBALS ////
const SCREEN_RATIO_W = 16;
const SCREEN_RATIO_H = 9;
const ASPECT_RATIO = SCREEN_RATIO_H/SCREEN_RATIO_W;
const CANVAS_OFFSET = 20;

var backdropColor = "black";
var displayBorderColor = "grey";
var starSize = 1;  //updated with screen size
const DEFAULT_NUM_STARS = 200;
var numStars = DEFAULT_NUM_STARS;
var starfield = [];
const MAX_CROSSING_TIME = 400;
const MIN_CROSSING_TIME = 150;
var starMaxSpeed = display.width/MIN_CROSSING_TIME; //updates with screen size
var starMinSpeed = display.width/MAX_CROSSING_TIME; //updates with screen size
const STAR_START_X = -100;

var deltaTime = 0;
var starDate = Date.now();
var starSpawnTimer = 0;
var starSpawnerInterval = .25;
var starSpawnBatchSize = 50;
const FRAME_RATE_TARGET = 60;

//Frame rate counter variables
var filterStrength = 20;
var frameTime = 0, lastLoop = new Date, thisLoop;
var fpsOut = 0;

//////////////////////////
//// EVENT LISTENERS ////
////////////////////////
window.addEventListener("keydown", onKeyDown, false);

function onKeyDown(ee){
	switch(ee){  //switch on relevant keycodes.
		default:
			break;
	}
}


///////////////////////
//	CONSTRUCTORS	//
/////////////////////

/*	obj Star()
*	constructor for the mobile object representing a star.
*
*/
function Star(xPos,yPos,xVel,yVel,starWidth,starHeight){
	this.x = xPos;
	this.y = yPos;
	this.vx = xVel;
	this.vy = yVel;
	this.width = starWidth;  //expressed as a portion of screen width.
	this.height = starHeight //expressed as a portion of screen height.
	this.color = "white";
}

Star.prototype.draw = function(){
	ctx.save(); //preserves the current context references.
	ctx.fillStyle = this.color;
	ctx.fillRect(this.x,this.y,this.width,this.height);
	ctx.restore();  //restores the context from saved reference.
}

Star.prototype.update = function(){
	this.x += this.vx;
	this.y += this.vy;
	if(this.x > display.width){
		this.x = STAR_START_X;
		this.y = utils.getRandomScreenXorY(display.height);
	}
}

//// DRAWING/RENDERING /////////////

function fitDisplayToWindow(){
	display.width = window.innerWidth;
	display.height = window.innerHeight -CANVAS_OFFSET;
	if(display.height/display.width < ASPECT_RATIO){ //window is too wide;
		display.width = display.height / ASPECT_RATIO;
	} else if(height/width > ASPECT_RATIO){ //window is too tall;
		display.height = display.width * ASPECT_RATIO -CANVAS_OFFSET;
	}

	//change screen-size dependant metrics;
	starSize = Math.max(display.width/500, 1);
	starMaxSpeed = display.width/MIN_CROSSING_TIME;
	starMinSpeed = display.width/MAX_CROSSING_TIME;
}

//main rendering pipeline
function drawFrame(){
	clearDisplay();
	drawBackdrop();

	drawStars();

	//HUD
	drawBorder();
	drawStarCounter();
	drawFrameRate();
}

function drawBackdrop(){
	ctx.fillStyle = backdropColor;
	ctx.fillRect(0, 0, display.width, display.height);
}

function drawBorder(){
	ctx.strokeStyle = displayBorderColor;
	ctx.lineWidth = 3;
	ctx.strokeRect(0,0,display.width,display.height);
}


//used this code i found on stackoverflow because it is slightly more optimized than my old clear function.
//@citation: user: Prestaul at http://stackoverflow.com/questions/2142535/how-to-clear-the-canvas-for-redrawing
function clearDisplay(){
// Store the current transformation matrix
ctx.save();

// Use the identity matrix while clearing the canvas
ctx.setTransform(1, 0, 0, 1, 0, 0);
ctx.clearRect(0, 0, display.width, display.height);

// Restore the transform
ctx.restore();
}



function drawStars(){
	for(var ii = 0, stars = starfield.length; ii<stars; ii++){
		starfield[ii].draw();
	}
}

function drawStarCounter(){
	ctx.save();
	ctx.font = "16px Arial";
	ctx.fillStyle = "white";
	ctx.fillText("Stars: " + starfield.length, display.width/2,starSize*10);
}


// @citation: Frame counting code taken from http://stackoverflow.com/questions/4787431/check-fps-in-js
function drawFrameRate(){
	ctx.save();
	ctx.fillStyle = "white";

	var thisFrameTime = (thisLoop=new Date) - lastLoop;
	frameTime+= (thisFrameTime - frameTime) / filterStrength;
	lastLoop = thisLoop;

	ctx.font = "16px Arial";
	ctx.fillText(fpsOut, starSize*10, starSize*10);
	ctx.restore();
}
setInterval(function(){fpsOut = (1000/frameTime).toFixed(1) + " fps";}, 1000);


///////////////////////////////////
//	DEMO SETUP AND UPDATE LOOP 	//
/////////////////////////////////

function generateStarfield(){
	for(var ii = 0; ii < numStars; ii++){
		addStar(utils.getRandomScreenXorY(display.width), utils.getRandomScreenXorY(display.height));
	}
	//console.log("Generated inital stars successfully: " + starfield.length);
}

function addStar(startX,startY){
	var star = new Star(startX,startY, 
						utils.getRandomFloat(starMinSpeed,starMaxSpeed), 
						0, starSize, starSize)
	starfield.push(star);
}

function removeStar(){ //pull a star that is off screen out of the array;
	//find offscreen star,
	//remove star using starfield.splice();
}

//program execution entry point
function onStart(){
	//resize the screen.
	fitDisplayToWindow();
	//generate stars
	generateStarfield();
	//begin update loop.
	update();
}



function updateStars(){
	//update each star's position
	for(var ii = 0, stars = starfield.length; ii<stars; ii++){
		starfield[ii].update();
	}
}

function spawnStars(){
	//check to see if we can add more stars, or if some need to be removed.
	starSpawnTimer += deltaTime;
	if(starSpawnTimer >= starSpawnerInterval){
		starSpawnTimer = 0;
		if(1000/frameTime > FRAME_RATE_TARGET){
			console.log("spawning " + starSpawnBatchSize + " stars..." + "FPS: " + fpsOut);
			for(var kk = 0; kk < starSpawnBatchSize; kk++){
				addStar(0, utils.getRandomScreenXorY(display.height));
			}
		}
	} else {
		//removeStar();
	}
}


function update(){
	window.requestAnimationFrame(update, display);
	deltaTime = (Date.now() - starDate ) / 1000;

	updateStars();
	spawnStars();

	//
	drawFrame();
	starDate = Date.now();
}


onStart();