using System;
using Melanchall.DryWetMidi.Interaction;
using Visual;

public partial class NoteData
{
    public Note Note { get; private set; }
    public int Index { get; private set; }
    public double TimeStamp { get; private set; }
    public NoteVisual NoteVisual { get; private set; }
    public EventHandler<double> onNoteMakeVisible;
    public EventHandler<bool> onNoteDestroy;

    public NoteData(Note note, int index, double timeStamp, NoteVisual noteVisual)
    {
        Note = note;
        Index = index;
        TimeStamp = timeStamp;
        NoteVisual = noteVisual;
    }
}