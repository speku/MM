package spielHaufen;

/**
 * Repräsentiert einen Spielzug.
 * Enthält nichts Aufregendes und ist deshalb nur sparsam dokumentiert.
 * 
 * @author Katrin Zetlmeisl
 */
public class Spielzug {
	/* Nummer des Haufen, von dem Elemente entfernt werden. Der erste Haufen hat die Nummer 0. */
	private int haufen;
	/* Anzahl der zu wegzunehmenden Elemente. Mindestens 1, höchstens verbleibende Größe des Haufens. */
	private int anzahl;
	
	
	public Spielzug(int haufen, int anzahl) {
		super();
		this.haufen = haufen;
		this.anzahl = anzahl;
	}
	public int getHaufen() {
		return haufen;
	}
	public void setHaufen(int haufen) {
		this.haufen = haufen;
	}
	public int getAnzahl() {
		return anzahl;
	}
	public void setAnzahl(int anzahl) {
		this.anzahl = anzahl;
	}
	
	public String toString() {
		return "Haufen " + haufen + " minus " + anzahl + " Elemente";
	}
}
