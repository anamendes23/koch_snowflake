public enum RenderCommand
{
    NONE,
    FORWARD,    // 'F' in traditional notation
    FORWARD2,   // 'R' in traditional notation
    IGNORE,     // 'X' in traditional notation
    RIGHT,      // '-' in traditional notation
    LEFT,       // '+' in traditional notation
    PUSH,       // '[' in traditional notation
    POP         // ']' in traditional notation
}

public class Rewriter
{
    private RenderCommand[] from;
    private RenderQueue[] to;

    /**
     * Create an L-system that has the given rules.
     * Example: if we had a one system rule: F --> FF, then from[0] would
     * be RenderCommand.FORWARD and to[0] would be RenderQueue.fromString("FF").
     * @param from the left side of the rules
     * @param to the corresponding right side of the rules
     */
    public Rewriter(RenderCommand[] from, RenderQueue[] to)
    {
        this.from = from;
        this.to = to;
    }

    /**
     * Perform the rewriting for the given number of generations.
     * @param seed starting word
     * @param numGenerations how many times to rewrite the word with our rules
     * @return the nth-generation word
     */
    public RenderQueue rewrite(RenderQueue seed, int numGenerations)
    {
        // Implementation is just to treat the seed as the "previous" output
        // queue and then move the output queue to be the input queue for
        // the next generation, apply the rules to any axioms for which we
        // have rules (the other axioms are copied over to the output
        // directly). Repeat this process for the given number of generations.
        RenderQueue output = seed.copy();
        RenderQueue input;
        for (int gen = 0; gen < numGenerations; gen++)
        {
            input = output; // last generation's output is this one's input
            output = new RenderQueue();
            while (!input.empty())
            {
                RenderCommand nextInput = input.dequeue();
                // look through the from-list to find a rule to invoke
                bool ruleFound = false;
                for (int i = 0; !ruleFound && i < from.Length; i++)
                    if (from[i] == nextInput)
                    {
                        ruleFound = true;
                        // append a copy of this rule's right side onto the 
                        // end of the output queue
                        output.append(to[i]);
                    }
                // if no rule was found, then just copy this command to output
                if (!ruleFound)
                    output.enqueue(nextInput);
            }
        }
        return output;
    }
}
