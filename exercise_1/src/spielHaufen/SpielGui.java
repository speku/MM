package spielHaufen;

import java.awt.Color;
import java.awt.GridLayout;
import java.awt.HeadlessException;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JFrame;

/**
 * Das Spielfenster ( diese Klasse enthält also die GUI).
 * 
 * @author Katrin Zetlmeisl
 */
public class SpielGui extends JFrame {
	
	/* nur damit die blöde Warnung in Eclipse weggeht - funktioniert alles auch ohne */
	private static final long serialVersionUID = -3639153119930637372L;
	
	/* In diesem Element steckt die Spiellogik. */
	Spiel spiel;
	/* Wie viele Haufen gbt es (Länge des Arrays) und wie viele Elemente haben sie (Inhalt des Arrays)? */
	int[] startHaufen; 
	/* Variable enthält die Größe des größten Haufens. Wird im Konstruktor gefüllt. */
	int maxLength = 0;
	/* Ist das Spiel bereits beendet? Manchmal muss man das einfach wissen. */
	boolean spielVorbei = false;
	
	// Die Buttons in der GUI:
	/* immer vorhanden - Spiel beenden und Fenster schließen */
	JButton buttonExit;  
	/* immer unsichtbar - wird verwendet als Auslöser für den Zug des Computers */
	JButton buttonComputer; 
	/* sichtbar am Spielende - zeigt an, wer gewonnen hat */
	JButton buttonErgebnis;
	/* sichtbar am Spielende - neues Spiel starten */
	JButton buttonNeu;
	/* Die anklickbaren Elemente der Haufen. Jeder Button repräsentiert ein Element. */
	JButton[][] elemente;
	

	public SpielGui(Spiel spiel, int[] startHaufen) throws HeadlessException {
		super();
		this.spiel = spiel;
		this.startHaufen = startHaufen;
		for (int laenge : startHaufen) {
			maxLength = Math.max(maxLength, laenge);
		}
		this.getContentPane().setLayout(new GridLayout(startHaufen.length + 1, Math.max(4, maxLength)));
		this.initWindow();
	}

	/**
	 * Initialisiert das Fenster.
	 * Fügt alle Buttons hinzu und aktiviert davon die passenden.
	 */
	private void initWindow() {
		this.elemente = new JButton[startHaufen.length][maxLength];
		for (int i = 0; i < startHaufen.length; i++) {
			elemente[i] = new JButton[maxLength];
			for (int j = 0; j < maxLength; j++) {
				elemente[i][j] = new JButton("(" + i + "," + j + ")");
				elemente[i][j].addActionListener(new MyActionListenerElement());
				this.getContentPane().add(elemente[i][j]);
				if (j >= startHaufen[i]) {
					elemente[i][j].setEnabled(false);
				}
			}
		}
		
		buttonErgebnis = new JButton("");
		this.getContentPane().add(buttonErgebnis);
		buttonErgebnis.setEnabled(false);
		buttonErgebnis.setVisible(false);
		
		buttonComputer = new JButton("Computer");
		buttonComputer.addActionListener(new MyActionListenerComputer());
		this.getContentPane().add(buttonComputer);
		buttonComputer.setVisible(false);
		
		buttonNeu = new JButton("nochmal!");
		buttonNeu.addActionListener(new MyActionListenerNeu());
		this.getContentPane().add(buttonNeu);
		buttonNeu.setVisible(false);
		
		buttonExit = new JButton("EXIT");
		buttonExit.addActionListener(new MyActionListenerExit());
		this.getContentPane().add(buttonExit);	
	}
	
	/**
	 * Findet die Position des übergebenen Buttons in der Tabelle:
	 * @param currentButton beliebiger Button aus dem Array elemente
	 * @return Array der Länge 2: Im ersten Element steht Zeile (also der Haufen), im zweiten Element die Spalte, in der sich der Button befindet.
	 */
	public int[] findPosition(JButton currentButton) {
		JButton[][] alleButtons = elemente;
		for (int i = 0; i < startHaufen.length; i++) {
			for (int j = 0; j < maxLength; j++) {
				if (currentButton.equals(alleButtons[i][j])) {
					return new int[]{i,j};
				}
			}
		}
		return null;
	}
	
	/**
	 * Führt den Spielzug des Computers aus.
	 * Hier können Sie eine geschicktere Methode aufrufen als die mit dem Random.
	 */
	public void macheSpielzugComputer() {
		// Spielzustand aus Buttons erzeugen
		Spielzustand zustandVorher = getSpielzustand();
		// schon fertig?
		if (zustandVorher.isLeer()) {
			System.out.println("Fertig! Spieler hat gewonnen!");
			spiel.beenden();
		}
		// kurz warten...
		try {
			Thread.sleep(1000);
		} catch (InterruptedException e) {
			// mir doch egal
		}
		// Spiel aufrufen
		Spielzustand zustandNachher = spiel.macheSpielzugComputerRandom(zustandVorher);
		System.out.println("Spielzug Computer: " + zustandVorher.getSpielzug(zustandNachher));
		// Buttons richtig enablen
		implementSpielzustand(zustandNachher);
		if (zustandNachher.isLeer()) {
			System.out.println("Fertig! Computer hat gewonnen!");
			setGewonnen("Computer", Color.RED);
		}
	}
	
	/**
	 * Führt den Spielzug des Spielers nach dessen Klick auf einen Elementbutton aus.
	 * @param currentButton geklicktes Element
	 */
	public void macheSpielzugSpieler(JButton currentButton) {
		Spielzustand zustandVorher = getSpielzustand();
		int[] position = findPosition(currentButton);
		int nummerHaufen = position[0];
		int nehmen = zustandVorher.getHaufen()[nummerHaufen] - position[1];
		Spielzug spielzug = new Spielzug(nummerHaufen, nehmen);
		Spielzustand zustandNachher = zustandVorher.macheSpielzug(spielzug);
		System.out.println("Spielzug Spieler: " + spielzug.toString());
		implementSpielzustand(zustandNachher);
		if (zustandNachher.isLeer()) {
			System.out.println("Fertig! Spieler hat gewonnen!");
			setGewonnen("Spieler", Color.CYAN);
		}
	}

	/**
	 * Ermittelt den Spielzustand aus der GUI.
	 * @return aktuell angezeigter Spielzustand
	 */
	public Spielzustand getSpielzustand() {	
		int[] haufen = new int[startHaufen.length];
		for (int i = 0; i < startHaufen.length; i++) {
			haufen[i] = maxLength;
			for (int j = 0; j < maxLength; j++) {
				if (!elemente[i][j].isEnabled()) {
					haufen[i] = j;
					break;
				}
			}
		}
		Spielzustand result = new Spielzustand(haufen);
		return result;
	}
	
	/**
	 * Deaktiviert die Elemente, um den übergebenen Spielzustand herzustellen.
	 * @param zustand anzuzeigender Spielzustand
	 */
	public void implementSpielzustand(Spielzustand zustand) {
		int anzahlHaufen = zustand.getHaufen().length;
		if (anzahlHaufen != startHaufen.length) {
			throw new IllegalArgumentException();
		}
		for (int i = 0; i < anzahlHaufen; i++) {
			for (int j = 0; j < maxLength; j++) {
				if (j >= zustand.getHaufen()[i]) {
					elemente[i][j].setEnabled(false);
				} else {
					elemente[i][j].setEnabled(true);
				}
			}
		}
	}
	
	/**
	 * Wird aufgerufen, wenn die Spielrunde vorbei ist.
	 * @param spieler Gewinner (wird angezeigt)
	 * @param color Farbe des Ergebnisbuttons (nur zum Spaß)
	 */
	public void setGewonnen(String spieler, Color color) {
		spielVorbei = true;
		buttonErgebnis.setText(spieler + " hat gewonnen!");
		buttonErgebnis.setForeground(Color.BLACK);
		buttonErgebnis.setBackground(color);
		buttonErgebnis.setEnabled(true);
		buttonErgebnis.setVisible(true);
		buttonNeu.setEnabled(true);
		buttonNeu.setVisible(true);
	}

	
	// Jetzt folgen die Listener für die unterschiedlichen Buttons:
	
	class MyActionListenerElement implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			JButton currentButton = (JButton) e.getSource();
			boolean isAktiv = currentButton.isEnabled();
			if (isAktiv) {
				macheSpielzugSpieler(currentButton);
				// jetzt ist der Computer dran
				if (!spielVorbei) {
					buttonComputer.doClick();
				}
			}
		}
	}

	class MyActionListenerComputer implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			macheSpielzugComputer();
		}
	}
	
	class MyActionListenerNeu implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			spiel.neu();
		}
	}
	
	class MyActionListenerExit implements ActionListener {
		public void actionPerformed(ActionEvent e) {
			spiel.beenden();
		}
	}

}