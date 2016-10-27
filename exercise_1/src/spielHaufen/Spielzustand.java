package spielHaufen;

import java.util.Arrays;

/**
 * Repräsentiert einen Spielzustand.
 * 
 * @author Katrin Zetlmeisl
 */
public class Spielzustand {
	
	/* Anzahl der Haufen, mit denen gespielt wird */
	private int ANZAHL_HAUFEN;
	/* Anzahlen der Elemente auf den einzelnen Haufen */
	private int[] haufen;
	
	/**
	 * Konstruktor
	 * @param haufen Für jeden Haufen die Anzahl der Elemente als Array.
	 */
	public Spielzustand(int[] haufen) {
		super();
		ANZAHL_HAUFEN = haufen.length;
		this.haufen = new int[ANZAHL_HAUFEN];
		for (int i = 0; i < ANZAHL_HAUFEN; i++) {
			if (haufen.length > i) {
				this.haufen[i] = haufen[i];
			} else {
			this.haufen[i] = haufen[i];
			}
		}
	}
	
	/**
	 * getter
	 * @return Anzahl der Elemente der einzelnen Haufen als Array.
	 */
	public int[] getHaufen() {
		return haufen;
	}
	
	/**
	 * Kann dieser Spielzug bei diesem Spielzustand durchgefürht werden?
	 * @param zug zu prüfender Spielzug
	 * @return Ist der Spielzug regelkonform?
	 */
	public boolean isGueltigerZug(Spielzug zug) {
		if (zug.getHaufen() < 0 || zug.getHaufen() >= ANZAHL_HAUFEN) {
			return false;
		}
		if (zug.getAnzahl() < 1) {
			return false;
		}
		if (haufen[zug.getHaufen()] < zug.getAnzahl()) {
			return false;
		}
		return true;
	}
	
	/**
	 * Wende den übergebenen Spielzug auf den Spielzustand an und gib den neuen Spielzustand zurück.
	 * @param zug Spielzug (regelkonform!)
	 * @return Spielzustand nach Spielzug
	 */
	public Spielzustand macheSpielzug(Spielzug zug) {
		if (!isGueltigerZug(zug)) {
			throw new IllegalArgumentException();
		}
		int[] neueHaufen = new int[haufen.length];
		for (int i = 0; i < haufen.length; i++) {
			if (i == zug.getHaufen()) {
				neueHaufen[i] = haufen[i] - zug.getAnzahl();
			} else {
				neueHaufen[i] = haufen[i];
			}
		}
		return new Spielzustand(neueHaufen);
	}
	
	/**
	 * Ermittelt den Spielzug, der noetig ist, um zum uebergebenen Spielzustand zu gelangen.
	 * Wird aktuell nicht aufgerufen, kann aber praktisch sein.
	 * @param zustand Zielzustand
	 * @return Spielzug
	 */
	public Spielzug getSpielzug(final Spielzustand zustand) {
		
		int[] alteHaufen = getHaufen();
		int[] neueHaufen = zustand.getHaufen();
		if (alteHaufen.length != neueHaufen.length) {
			return null;
		}
		
		int nrUnterschiedlicherHaufen = -1;
		int differenz = 0;
		for (int i = 0; i < alteHaufen.length; i++) {
			int diff = alteHaufen[i] - neueHaufen[i];
			if (diff < 0 || diff > alteHaufen[i]) {
				return null;
			} else if (diff > 0) {
				if (nrUnterschiedlicherHaufen != -1) {
					return null;
				}
				differenz = diff;
				nrUnterschiedlicherHaufen = i;
			}
		}
		
		return new Spielzug(nrUnterschiedlicherHaufen, differenz);
	}
	
	/**
	 * Sind schon alle Elemente weg?
	 * @return true, falls alle Elemente weg sind
	 */
	public boolean isLeer() {
		int summe = 0;
		for (int i = 0; i < ANZAHL_HAUFEN; i++) {
			summe += haufen[i];
		}
		return (summe == 0);
	}
	
	/**
	 * für die Ausgabe (Logging und Debugging)
	 */
	public String toString() {
		StringBuilder buf = new StringBuilder();
		buf.append("(");
		for (int i = 0; i < ANZAHL_HAUFEN; i++) {
			buf.append(haufen[i] + ", ");
		}
		if (buf.length() > 2) {
			buf.delete(buf.length() - 2, buf.length());
		}
		buf.append(")");
		return buf.toString();
	}
}
