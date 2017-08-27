public
var pathToSlides = "";
public
var currSlide = 0;
public
var slides: GameObject[];
private
var sections: GameObject[];
private
var slidesNum: int;


function Start() {
    // set the camera field of view to be compatible with the screen and keep the FOVhorizontal constant
    GetComponent.<Camera>().fieldOfView = GetComponent.<Camera>().fieldOfView * 16 / 9 * 1 / GetComponent.<Camera>().aspect;

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

    // Get all slides in the folder
    // var info: String[] = System.IO.Directory.GetFiles("E:/Git/slides-of-loci/Assets/Coginfocom2017/Slides/coginfocom2017_slides/", "*.png");


    for (var i = 0; i < sections.Length; i++) {
        for (var child: Transform in sections[i].transform) {
            if (child.gameObject.tag == "Slide") {
                child.gameObject.GetComponent.<Camera>().enabled = false; // disable all cameras on start
                slides += [child.gameObject];
                var filePath: String = pathToSlides + slides.length + ".png";

                if (System.IO.File.Exists(filePath))     {
                    fileData = System.IO.File.ReadAllBytes(filePath);
                    var tex: Texture2D;
                    tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
                    tex.LoadImage(fileData);
                    child.GetChild(0).GetComponent.<Renderer>().material.mainTexture = tex;
                }
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
    }

    if (Input.GetKeyDown(KeyCode.Space)) {
        if (currSlide < slidesNum - 1) { currSlide += 1; } //slidenumber has to be in the range of slides
    }

    if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        if (currSlide > 0) { currSlide -= 1; } //slidenumber has to be in the range of slides
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
    RenderSettings.skybox = slides[currSlide].GetComponent.<Camera>().GetComponent(Skybox).material;
    GetComponent.<Camera>().nearClipPlane = slides[currSlide].GetComponent.<Camera>().nearClipPlane;
    GetComponent.<Camera>().farClipPlane = slides[currSlide].GetComponent.<Camera>().farClipPlane;


    /// transition controller
    var transitionTime = slides[currSlide].GetComponent(Attributes).transitionTime;
    

    if (transitionTime > 0) {
    	// introducing a small asynchrony between the translation and the rotation
        transform.position = Vector3.Lerp(transform.position, slides[currSlide].transform.position, Time.deltaTime / transitionTime * 1.15);
        transform.rotation = Quaternion.Lerp(transform.rotation, slides[currSlide].transform.rotation, Time.deltaTime / transitionTime);
    } else {
        transform.position = slides[currSlide].transform.position;
        transform.rotation = slides[currSlide].transform.rotation;
    }
}


function Compare(go1: GameObject, go2: GameObject): int {
    // compare function to sort the sections
    return go1.name.CompareTo(go2.name);
}
