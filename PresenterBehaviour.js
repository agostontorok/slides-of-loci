public
var currSlide = 0;
public
var slides: GameObject[];
public
var sections: GameObject[];
private
var startTime: float;
private
var slidesNum: int;

function Start() {
	// set the camera field of view to be compatible with the screen and keep the FOVhorizontal constant
	camera.fieldOfView = camera.fieldOfView * 16/9 * 1/camera.aspect;

    /*
	We use two tags to reference and organize slides (scenes) : Slide and Section

	Slides should be organized in sections. Sections are read ordered by how they appear in the
	hierarchy and then we iterate through slides inside each sectino and get them in the order they 
	appear in the editor window's hierarchy

	You have to create at least 1 section and 1 slide for a slideshow
    */
    slidesNum = GameObject.FindGameObjectsWithTag("Slide").Length;

    sections = GameObject.FindGameObjectsWithTag("Section");
    System.Array.Sort(sections, Compare);

    for (var i = 0; i < sections.Length; i++) {
        for (var child: Transform in sections[i].transform) {
            if (child.gameObject.tag == "Slide") {
                child.gameObject.camera.enabled = false; // disable all cameras on start
                slides += [child.gameObject];
            }
        }
    }
}


function Update() {
	/*
	You can use the LEFT and RIGHT keys to navigate between slides
	*/

    if (Input.GetKeyDown(KeyCode.RightArrow)) {
        if (currSlide < slidesNum - 1) { currSlide += 1; } //slidenumber has to be in the range of slides
        startTime = Time.time;
    }

    if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        if (currSlide > 0) { currSlide -= 1; } //slidenumber has to be in the range of slides
        startTime = Time.time;
    }

    /*
	Slide transitions:
	Transition is either 0 or some positive float. If zero no animation happens, if the number is bigger then zero, then
	the transition time attribute of the current slide will control the speed of the transition. 

	The camera that goes through the scenes will inherit some attributes of the camera of the given slides.
	Currently these are:
	 	-  clipping planes
	 	-  skybox

    */
    

    /// property inheriting
    RenderSettings.skybox = slides[currSlide].camera.GetComponent(Skybox).material;
    camera.nearClipPlane = slides[currSlide].camera.nearClipPlane;
    camera.farClipPlane = slides[currSlide].camera.farClipPlane;


    /// transition controller
    var transitionTime = slides[currSlide].GetComponent(Attributes).transitionTime;
    var distCovered = (Time.time - startTime) / transitionTime;
    var fracJourney = distCovered;
    if (transitionTime > 0) {
        transform.position = Vector3.Lerp(transform.position, slides[currSlide].transform.position, fracJourney + .15);
        transform.rotation = Quaternion.Lerp(transform.rotation, slides[currSlide].transform.rotation, fracJourney);
    } else {
        transform.position = slides[currSlide].transform.position;
        transform.rotation = slides[currSlide].transform.rotation;
    }
}


function Compare(go1: GameObject, go2: GameObject): int {
    // compare function to sort the sections
    return go1.name.CompareTo(go2.name);
}