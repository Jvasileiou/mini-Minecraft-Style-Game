using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBrain : MonoBehaviour
{
    public System.Random rnd = new System.Random();

    private static int[,] coordinatesArr = new int[100,3];
    public static int counter = 0 ;

    public GameObject sideLeft ;
    public GameObject sideFront ;
    public GameObject sideRight;
    public GameObject sideBack;
    public Material transparentMat;
    public GameObject spotLightLeft;
    public GameObject spotLightRight;
    public GameObject cylinderObject;

    // Blue , Green , LightBlue , Red , Yellow
    public GameObject[] cubesArr;
    
    private int rndNumber;
    public static int N ;
    public static int maxN;
    private static float stepX = 1;

    private static float oldY;
    private static float newY;
   
    private static RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        maxN = N+1;
        if (N > 1)
        {
            createTheFirstCubes();
        }
        
        // Useless values
        coordinatesArr[counter,0] = -1500;
        coordinatesArr[counter,1] = -1300;
        coordinatesArr[counter,2] = -1500;

        counter++;
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
        if (Physics.Raycast(ray, out hit, 4) && hit.transform.name != "Floor" && hit.transform.name != "CubeMagenta" && hit.transform.name != "CubeBlue")
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 2.5f);

            if (Input.GetKeyDown("p") )
            {
                doTheFourthQuestion();
            }

            if (Input.GetKeyDown("b"))
            {
                if (StackCubeScript.numberOfCubes > 0)
                {
                    doTheFifthQuestion();
                }              
            }

            if (Input.GetKeyDown("c"))
            {
                if (CylinderStackScript.numberOfCylinders > 0)
                {
                    doTheSixthQuestion();
                }
            }
        }
    }

    public void doTheSixthQuestion()
    {
        GameObject currentCylinderObject;

        float x = hit.transform.position.x;
        float y = hit.transform.position.y;
        float z = hit.transform.position.z;

        currentCylinderObject = Instantiate(cylinderObject);

        // MAYBE Check if there is one more 
        ScoreScript.scoreValue += 20;
        CylinderStackScript.numberOfCylinders--;
        currentCylinderObject.transform.position = new Vector3(x, y + 1, z);
    }

    public void doTheFifthQuestion()
    {
        GameObject cubeObject;

        rndNumber = rnd.Next(5);

        float x = hit.transform.position.x;
        float y = hit.transform.position.y+1.0f;
        float z = hit.transform.position.z;

        int parameterX = (int)Mathf.Round(x);
        int parameterY = (int)Mathf.Round(y);
        int parameterZ = (int)Mathf.Round(z);

        // add the coordinates of the position to the array2d
        if (!areThereTheseCoordinates(parameterX, parameterY, parameterZ))
        {
            Debug.Log("There is any cube in the +1 level");

            coordinatesArr[counter, 0] = parameterX;
            coordinatesArr[counter, 1] = parameterY ;
            coordinatesArr[counter, 2] = parameterZ;

            counter++;
            cubeObject = Instantiate(cubesArr[rndNumber]);

            // Check if there is one more 
            ScoreScript.scoreValue += 10;
            StackCubeScript.numberOfCubes--;

            cubeObject.transform.position = new Vector3(x, y, z);
        }
        else if (!areThereTheseCoordinates(parameterX, parameterY + 1, parameterZ))
        {
            Debug.Log("There is any cube in the +2 level");

            coordinatesArr[counter, 0] = parameterX;
            coordinatesArr[counter, 1] = parameterY + 1;
            coordinatesArr[counter, 2] = parameterZ;

            counter++;
            cubeObject = Instantiate(cubesArr[rndNumber]);

            // Check if there is one more 
            ScoreScript.scoreValue += 10;
            StackCubeScript.numberOfCubes--;

            cubeObject.transform.position = new Vector3(x, y + 1, z);
        }
        
    }

    private bool areThereTheseCoordinates(int x , int y , int z)
    {
        int arrX;
        int arrY;
        int arrZ;
        for (int i = 0; i<counter; i++)
        {
            arrX = coordinatesArr[i, 0] ;
            arrY = coordinatesArr[i, 1] ;
            arrZ = coordinatesArr[i, 2] ;

            Debug.Log("x = " + x);
            Debug.Log("y = " + y);
            Debug.Log("z = " + z);
            
            if ( (arrX == x) && (arrY == y) && (arrZ == z) )
            {
                return true;
            }
        }
       return false;
    }

    public static void findTheHeighOfTheJump(float old_y, float new_y)
    {
        oldY = old_y;
        newY = new_y;

        if (oldY == newY)
        {
            // Do nothing
        }
        // Fall    
        else if (oldY > newY)
        {
            decreaseTheScore();
        }
        // Jump
        else
        {
            increaseTheScore();
        }
    }

    public static void decreaseTheScore()
    {
        float difference = oldY - newY;
        Debug.Log("Difference = " + difference);

        if (difference < 1.05f) // fall only one level
            Debug.Log("NO INCREASE OF THE SCORE");
        else
        {
            float limit = 2.05f;
            // we have to decrease the score
            int i = 1;
            while (true)
            {
                if (difference < limit)
                {
                    ScoreScript.scoreValue -= (i * 10);
                    break;
                }
                i++;
                limit += 1.0f;
            }
        }
    }

    public static void decreaseTheHearts()
    {
        if (HeartsScript.heartsValue > 0)
        {
            HeartsScript.heartsValue -= 1;
        }
        else
        {
            Debug.Log("GAME OVER");
            Application.Quit();
        }      
    }

    public static bool istheScoreBiggestThan(float limit)
    {
        if (ScoreScript.scoreValue >= limit) return true;

        return false;
    }

    public static void increaseTheScore()
    {   
        if (newY > maxN)
        {    
            Debug.Log("YOU WON one level");
            //Application.Quit();
            HeartsScript.heartsValue += 1;
            ScoreScript.scoreValue += 100;

            maxN = maxN * 2;
        }

        float difference = newY - oldY;

        if (difference > 0.8f )
        {
            ScoreScript.scoreValue += 10;
        }
    }

    private void doTheFourthQuestion()
    {
        string currentColor;
        int currentVirtualCubes;
        // Green = 3 virtual cubes
        if (hit.transform.name == "CubeGreen(Clone)")
        {
            CubeGreenStatus other;
            other = hit.transform.GetComponent<CubeGreenStatus>();
       
            if(other.virtualCubes == 3) // GREEN
            {
                StackCubeScript.numberOfCubes++;
                ScoreScript.scoreValue -= 5;
                other.virtualCubes--;
                other.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            }
            else if (other.virtualCubes == 2) // RED
            {
                StackCubeScript.numberOfCubes++;
                ScoreScript.scoreValue -= 5;
                other.virtualCubes--;
                other.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
            }
            else if (other.virtualCubes == 1) // YELLOW
            {
                StackCubeScript.numberOfCubes++;
                ScoreScript.scoreValue -= 5;
                other.virtualCubes--;
                other.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
            }
            else if (other.virtualCubes == 0)
            {
                Debug.Log("IT HAS NOT ANY VIRTUAL CUBES");
            }
        }
        //  Red  = 2 virtual cubes
        else if (hit.transform.name == "CubeRed(Clone)")
        {
            CubeRedStatus other;
            other = hit.transform.GetComponent<CubeRedStatus>();

            if (other.virtualCubes == 2) // RED
            {
                StackCubeScript.numberOfCubes++;
                ScoreScript.scoreValue -= 5;
                other.virtualCubes--;
                other.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
            }
            else if (other.virtualCubes == 1) // YELLOW
            {
                StackCubeScript.numberOfCubes++;
                ScoreScript.scoreValue -= 5;
                other.virtualCubes--;
                other.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
            }
            else if (other.virtualCubes == 0) 
            {
                Debug.Log("IT HAS NOT ANY VIRTUAL CUBES");
            }
        }
        // Yellow = 1 virtual cubes
        else if (hit.transform.name == "CubeYellow(Clone)")
        {
            CubeYellowStatus other;
            other = hit.transform.GetComponent<CubeYellowStatus>();

            if (other.virtualCubes == 1) // YELLOW
            {
                StackCubeScript.numberOfCubes++;
                ScoreScript.scoreValue -= 5;
                other.virtualCubes--;
                other.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
            }
            else if (other.virtualCubes == 0)
            {
                Debug.Log("IT HAS NOT ANY VIRTUAL CUBES");
            }

        }
        // Light Blue Case 
        else if (hit.transform.name == "CubeLightBlue(Clone)")
        {
            CubeCyanStatus other;
            other = hit.transform.GetComponent<CubeCyanStatus>();

            GameObject lol;
            lol = hit.collider.gameObject;
            // We will remove it
            if (other.virtualCubes == 10) // CYAN
            {
                CylinderStackScript.numberOfCylinders++;
                ScoreScript.scoreValue -= 5;
                //other.GetComponent<MeshRenderer>().material = transparentMat ;
                other.virtualCubes = 0;

                Destroy(lol);
            }
            
        }

    }

    private void createTheFirstCubes()
    {
        GameObject currentCube;
        float stepZ = 1.0f;
        float cubeHeight = 1.5f ;
        bool isUp = true;

        for (int i = 1; i < N; i++){
            rndNumber = rnd.Next(5);
            currentCube = Instantiate(cubesArr[rndNumber]);

            if (isUp){
                currentCube.transform.position = new Vector3(0.0f, cubeHeight, stepZ);
                isUp = false;
            }
            else{
                currentCube.transform.position = new Vector3(0.0f, cubeHeight, -stepZ);
                isUp = true;
                stepZ += 1.0f;
            }
        }

        stepZ = 0;
        bool isLeft = true;
        for (int i = 1; i < N; i++)
        {
            if (isLeft)
            {
                rndNumber = rnd.Next(5);
                currentCube = Instantiate(cubesArr[rndNumber]);
                currentCube.transform.position = new Vector3(-stepX, cubeHeight, stepZ);

                stepZ = 1.0f;
                isUp = true;
                for (int j = 1; j < N; j++)
                {
                    rndNumber = rnd.Next(5);
                    currentCube = Instantiate(cubesArr[rndNumber]);

                    if (isUp)
                    {
                        currentCube.transform.position = new Vector3(-stepX, cubeHeight, stepZ);
                        isUp = false;
                    }
                    else
                    {
                        currentCube.transform.position = new Vector3(-stepX, cubeHeight, -stepZ);
                        isUp = true;
                        stepZ += 1.0f;
                    }
                }

                stepZ = 0.0f;
                isLeft = false;
            }
            else
            {
                rndNumber = rnd.Next(5);
                currentCube = Instantiate(cubesArr[rndNumber]);
                currentCube.transform.position = new Vector3(stepX, cubeHeight, stepZ);

                stepZ = 1.0f;
                isUp = true;
                for (int j = 1; j < N; j++)
                {
                    rndNumber = rnd.Next(5);
                    currentCube = Instantiate(cubesArr[rndNumber]);

                    if (isUp)
                    {
                        currentCube.transform.position = new Vector3(stepX, cubeHeight, stepZ);
                        isUp = false;
                    }
                    else
                    {
                        currentCube.transform.position = new Vector3(stepX, cubeHeight, -stepZ);
                        isUp = true;
                        stepZ += 1.0f;
                    }
                }

                stepZ = 0.0f;
                isLeft = true;
                stepX += 1.0f;
            }
        }
        stepX += 1.0f;

        createTheSideLimits();
        createTheSpotLights();
    }

    private void createTheSideLimits()
    {
        GameObject leftWall, rightWall;
        GameObject frontWall, backWall;

        leftWall = Instantiate(sideLeft);
        leftWall.transform.position = new Vector3(-stepX, 15, 0);

        rightWall = Instantiate(sideRight);
        rightWall.transform.position = new Vector3(stepX, 15, 0);

        frontWall = Instantiate(sideFront);
        frontWall.transform.position = new Vector3(0, 15, stepX);

        backWall = Instantiate(sideBack);
        backWall.transform.position = new Vector3(0, 15, -stepX);

    }

    private void createTheSpotLights()
    {
        GameObject spotLightL, spotLightR;
        int height = N + 1 ; 
        height++;

        spotLightL = Instantiate(spotLightLeft);
        spotLightL.transform.position = new Vector3(-stepX, height, stepX);

        spotLightR = Instantiate(spotLightRight);
        spotLightR.transform.position = new Vector3(stepX, height, -stepX);

    }
}
