using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public partial class PresenterBehaviour : MonoBehaviour
{
    public int currSlide;
    public List<GameObject> slides = new List<GameObject>();
    private GameObject[] sections;
    private int slidesNum;
    public virtual void Start()
    {
        // set the camera field of view to be compatible with the screen and keep the FOVhorizontal constant
        this.GetComponent<Camera>().fieldOfView = (((this.GetComponent<Camera>().fieldOfView * 16) / 9) * 1) / this.GetComponent<Camera>().aspect;
        /*
        We use two tags to reference and organize slides (scenes) : Slide and Section

        Slides should be organized in sections. Sections are read ordered by how they appear in the
        hierarchy and then we iterate through slides inside each sectino and get them in the order they
        appear in the editor window's hierarchy

        You have to create at least 1 section and 1 slide for a slideshow
           */
        this.slidesNum = GameObject.FindGameObjectsWithTag("Slide").Length;
        this.sections = GameObject.FindGameObjectsWithTag("Section");

        System.Array.Sort(this.sections, this.Compare);
        int i = 0;
        while (i < this.sections.Length)
        {
            foreach (Transform child in this.sections[i].transform)
            {
                if (child.gameObject.tag == "Slide")
                {
                    child.gameObject.GetComponent<Camera>().enabled = false; // disable all cameras on start
                    this.slides.Add(child.gameObject);
                }
            }
            i++;
        }
    }

    public virtual void Update()
    {
        /*
        You can use the LEFT and RIGHT keys to navigate between slides
        */
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (this.currSlide < (this.slidesNum - 1))
            {
                this.currSlide = this.currSlide + 1; //slidenumber has to be in the range of slides
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (this.currSlide < (this.slidesNum - 1))
            {
                this.currSlide = this.currSlide + 1; //slidenumber has to be in the range of slides
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (this.currSlide > 0)
            {
                this.currSlide = this.currSlide - 1; //slidenumber has to be in the range of slides
            }
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
        RenderSettings.skybox = ((Skybox) this.slides[this.currSlide].GetComponent<Camera>().GetComponent(typeof(Skybox))).material;
        this.GetComponent<Camera>().nearClipPlane = this.slides[this.currSlide].GetComponent<Camera>().nearClipPlane;
        this.GetComponent<Camera>().farClipPlane = this.slides[this.currSlide].GetComponent<Camera>().farClipPlane;
        /// transition controller
        float transitionTime = ((Attributes) this.slides[this.currSlide].GetComponent(typeof(Attributes))).transitionTime;
        if (transitionTime > 0)
        {
            // introducing a small asynchrony between the translation and the rotation
            this.transform.position = Vector3.Lerp(this.transform.position, this.slides[this.currSlide].transform.position, (Time.deltaTime / transitionTime) * 1.15f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.slides[this.currSlide].transform.rotation, Time.deltaTime / transitionTime);
        }
        else
        {
            this.transform.position = this.slides[this.currSlide].transform.position;
            this.transform.rotation = this.slides[this.currSlide].transform.rotation;
        }
    }

    public virtual int Compare(GameObject go1, GameObject go2)
    {
        // compare function to sort the sections
        return go1.name.CompareTo(go2.name);
    }

}