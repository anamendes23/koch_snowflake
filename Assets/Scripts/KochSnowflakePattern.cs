using System;
using UnityEngine;

public class KochSnowflakePattern : MonoBehaviour
{
    public int numGenerations;
    public float forward;
    public int turn;
    public float startX;
    public float startY;
    public double startHeading;

    public float step;

    Vector3 pos;
    private RenderQueue word;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.localPosition;

        step = 10 * Time.deltaTime; // calculate distance to move
        forward = 2.0f;
        turn = 60;
        startX = pos.x;
        startY = pos.y;
        startHeading = 60;

        setUp();
    }

    // Update is called once per frame
    void Update()
    {
        float diff = Vector3.Distance(transform.position, pos);
        //Debug.Log(diff);
        if (diff < 0.001f)
        {
            //Debug.Log("X: " + pos.x + " Y: " + pos.y);
            updatePosition();
            //Debug.Log("X: " + pos.x + " Y: " + pos.y);
            transform.position = Vector3.MoveTowards(transform.position, pos, step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, step);
        }
    }

    private void setUp()
    {
        numGenerations = 4;

        word = getKochSnowflake(numGenerations);
    }

    /**
     * Make a Koch Snowflake L-system.
     * @param numGenerations number of rewrite generations
     * @return the word for rendering
     */
    private RenderQueue getKochSnowflake(int numGenerations)
    {
        // rule: F --> F-F++F-F
        RenderCommand[] ruleFrom = { RenderCommand.FORWARD };
        RenderQueue[] ruleTo = { RenderQueue.fromString("F-F++F-F") };
        Rewriter rewriter = new Rewriter(ruleFrom, ruleTo);

        RenderQueue seed = RenderQueue.fromString("F++F++F");
        return rewriter.rewrite(seed, numGenerations);
    }

    private void updatePosition()
    {
        RenderCommand rc = RenderCommand.NONE;
        bool updatedPos = false;

        while (!(word.empty()) && !updatedPos)
        {
            rc = word.dequeue();
            switch (rc)
            {
                case RenderCommand.FORWARD:
                case RenderCommand.FORWARD2:
                    float x;
                    float y;

                    double theta = Math.Atan2(pos.y, pos.x);
                    x = (float)(forward * Math.Cos(theta));
                    y = (float)(forward * Math.Sin(theta));

                    //Debug.Log("THETA: X: " + x + " Y: " + y);

                    double r = Math.Sqrt(x * x + y * y);
                    x = (float)(r * Math.Cos(startHeading));
                    y = (float)(r * Math.Sin(startHeading));
                    //Debug.Log("R: X: " + x + " Y: " + y);

                    pos.x += x;
                    pos.y += y;

                    updatedPos = true;
                    break;

                case RenderCommand.RIGHT:
                    startHeading += (turn * Math.PI / 180.0);
                    break;

                case RenderCommand.LEFT:
                    startHeading += (-turn * Math.PI / 180.0);
                    break;

                case RenderCommand.PUSH:
                case RenderCommand.POP:
                case RenderCommand.IGNORE:
                default:
                    break;
            }

            word.enqueue(rc);
        }
    }
}
