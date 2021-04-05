using System;
using UnityEngine;

public class KochSnowflakePatternConcentric : MonoBehaviour
{
    public float forward;
    public float speed;

    private int numGenerations;
    private int turn;
    private double startHeading;
    private float startX;
    private float startY;
    private float step;
    private float scaleFactor;

    private Vector3 pos;
    private RenderQueue word;
    private int queueSize;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.localPosition;
        Debug.Log(pos.x + " " + pos.y);
        //forward = 2.0f;
        startX = pos.x;
        startY = pos.y;
        step = speed * Time.deltaTime;
        scaleFactor = 1.2f;

        setUp();
        queueSize = word.getSize();
    }

    // Update is called once per frame
    void Update()
    {
        float diff = Vector3.Distance(transform.position, pos);

        if (!word.empty())
        {
            if (diff < 0.001f)
            {
                updatePosition();
                transform.position = Vector3.MoveTowards(transform.position, pos, step);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, step);
            }
        }
        else
        {
            setUp();
        }
    }

    private void setUp()
    {
        turn = 60;
        startHeading = 60;
        numGenerations = 4;

        word = getKochSnowflake(numGenerations);
    }

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

            if(word.getSize() == queueSize / 2)
                Debug.Log(pos.x + " " + pos.y);
        }
    }
}
