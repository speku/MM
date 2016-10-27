package spielHaufen;

import java.util.stream.Stream;
import java.util.stream.Collector;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.IntSummaryStatistics;
import java.util.List;
import java.util.Random;
import java.util.Map;
import java.util.stream.IntStream;


/**
 * Die Klasse mit der main-Methode. Bitte hier starten!
 * Diese Klasse enth‰lt auﬂerdem die Logik des Spiels.
 * 
 * @author Katrin Zetlmeisl
 */
public class Spiel {

	public static void main(String[] args) {
		Spiel spiel = new Spiel();
		SpielGui gui = new SpielGui(spiel, new int[]{7,5,3}); // Hier stellt man die initiale Haufengrˆﬂe ein.
		gui.setBounds(0, 0, 700, 300);
		gui.setVisible(true);
	}

	/**
	 * Ermittelt einen regelkonformen aber zuf‰lligen Spielzug, den der Computer macht, wenn er dran ist.
	 * Ersetzen Sie diesen durch einen zielf¸hrenden Spielzug!
	 * Daf¸r kˆnnen Sie entweder diese Methode ‰ndern oder einen neue schreiben und statt dieser aufrufen.
	 * @param zustandVorher vorgefundener Spielzustand
	 * @return neuer Spielzustand nach dem Zug
	 */
	public Spielzustand macheSpielzugComputerRandom(Spielzustand zustandVorher) {
		int[] heap = zustandVorher.getHaufen();
		Arrays.stream(heap).mapToObj(n -> {return permutations(++i,n,heap);});
		
		
	}
	
	private int[][] permutations(int i, int n, int[] heap){
		IntStream.rangeClosed(0, n - 1).map(x -> heap[i] = x);
		
	
	
	return null;
	}
	
	
	
	
	/**
	 * Wird aufgerufen, wenn man auf den Button nochmal! klickt.
	 * Startet eine neue Spielrunde in einem neuen Fenster.
	 */
	public void neu() {
		// TODO altes Fenster schlieﬂen
		SpielGui gui = new SpielGui(this, new int[]{7,5,3});
		gui.setBounds(0, 0, 700, 300);
		gui.setVisible(true);
	}
	
	/**
	 * Beendet den gesamten Prozess und schlieﬂt alle Fenster.
	 */
	public void beenden() {
		System.exit(0);
	}
}
