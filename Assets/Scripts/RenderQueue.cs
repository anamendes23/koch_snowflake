using System.Collections.Generic;
using System.Text;
using System;

public class RenderQueue
{
    private CommandNode head;
    private CommandNode tail;
    private int size;
    private static Dictionary<char, RenderCommand> dictionary;

    public RenderQueue()
    {
        head = new CommandNode(RenderCommand.NONE, null, null);
        tail = new CommandNode(RenderCommand.NONE, null, null);
        head.next = tail;
        tail.prev = head;

        size = 0;
        populateDictionary();
    }

    private void populateDictionary()
    {
        dictionary = new Dictionary<char, RenderCommand>();

        dictionary.Add('F', RenderCommand.FORWARD);
        dictionary.Add('R', RenderCommand.FORWARD2);
        dictionary.Add('X', RenderCommand.IGNORE);
        dictionary.Add('-', RenderCommand.RIGHT);
        dictionary.Add('+', RenderCommand.LEFT);
        dictionary.Add('[', RenderCommand.PUSH);
        dictionary.Add(']', RenderCommand.POP);
    }

    public void enqueue(RenderCommand command)
    {
        CommandNode newNode = new CommandNode(command, null, null);
        tail.prev.next = newNode;
        newNode.prev = tail.prev;
        tail.prev = newNode;
        newNode.next = tail;

        size++;
    }

    public RenderCommand dequeue()
    {
        // get node
        CommandNode returnNode = head.next;

        //remove node from queue
        head.next = returnNode.next;
        returnNode.next.prev = head;

        size--;

        return returnNode.value;
    }

    public void append(RenderQueue queue)
    {

        CommandNode curr = queue.head.next;

        while (curr != null && curr.next != null)
        {
            enqueue(curr.value);
            curr = curr.next;
        }
    }

    public RenderQueue copy()
    {

        RenderQueue copyQueue = new RenderQueue();
        copyQueue.append(this);

        return copyQueue;
    }

    public bool empty()
    {
        return size == 0;
    }

    public int getSize()
    {
        return size;
    }

    public static RenderQueue fromString(string commands)
    {

        RenderQueue queue = new RenderQueue();

        foreach (char c in commands)
        {
            queue.enqueue(dictionary[c]);
        }

        return queue;
    }

    public string toString()
    {
        //Declare a StringBuilder object to create a String
        StringBuilder sb = new StringBuilder();
        //sb.Append("RenderQueue: ");

        //Declare a Node object to traverse through input queue
        CommandNode curr = head.next;

        //While queue has more elements to be translated
        while (curr != null && curr.next != null)
        {
            //Get the char representation of the command
            sb.Append(getChar(curr.value));
            sb.Append("\n");

            //update point to evaluate next node
            curr = curr.next;
        }

        //Return the String representation of the queue
        return sb.ToString();
    }

    private char getChar(RenderCommand command)
    {
        //If else statements checking input and returning corresponding char
        if (command == RenderCommand.FORWARD)
        {
            return 'F';
        }
        else if (command == RenderCommand.FORWARD2)
        {
            return 'R';
        }
        else if (command == RenderCommand.IGNORE)
        {
            return 'X';
        }
        else if (command == RenderCommand.RIGHT)
        {
            return '-';
        }
        else if (command == RenderCommand.LEFT)
        {
            return '+';
        }
        else if (command == RenderCommand.PUSH)
        {
            return '[';
        }
        else if (command == RenderCommand.POP)
        {
            return ']';
        }
        else
        {
            //If input is unknown, throw an exception
            throw new ArgumentException("Command not supported.");
        }
    }

    protected class CommandNode
    {
        public RenderCommand value;
        public CommandNode prev;
        public CommandNode next;

        public CommandNode(RenderCommand val, CommandNode prev, CommandNode next)
        {
            value = val;
            this.prev = prev;
            this.next = next;
        }
    }
}
