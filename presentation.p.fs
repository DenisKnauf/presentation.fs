
\ Haeufig benutzte Textauszeichnungen:
: <f> <b> Blue <fc> ;
: </f> </fc> </b> ;

: p4-1
	<h> !" Welche Funktionen sind moeglich?" </h>
	<p>
		!" Natuerlich Text: " <f> s\" !\" Irgend ein Text\"" !! </f>
		<br>
		!" (Manchmal ist '" <f> s\" s\" Etwa wenn man !\" erklaeren will\" !!" !! </f> !" ' noetig)"
	</p>
;
: p4-2
	<p> !" Aber " <u> !" immer" </u> !"  innerhalb eines Blockes:" </p>
	<li> <f> s\" <h> !\" Eine Ueberschrift\" </h>" !! </f> </li>
	<li> <f> s\" <p> !\" Einfacher Text\" </p>" !! </f> </li>
	<li> <f> s\" <li> !\" Listen, wie diese hier\" </li>" !! </f> </li>
;

: p4-3
	<p> !" Textauszeichnung:" </p>
	<li> <f> s\" <b> !\" Fettdruck\" </b>" !! </f> !" : " <b> !" Fettdruck" </b> </li>
	<li> <f> s\" <i> !\" Farbinvertierung\" </i>" !! </f> !" : " <i> !" Farbinvertierung" </i> </li>
	<li> <f> s\" <u> !\" Unterstrichen\" </u>" !! </f> !" : " <u> !" Unterstrichen" </u> </li>
;

: farbendemo'' <fc> !" ====" ;
: farbendemo'
	7 0 +do
		i postpone literal postpone <bc>
		7 0 +do
			i postpone literal postpone farbendemo''
		loop
		postpone <br>
	loop
; immediate
: farbendemo   farbendemo' </bc> </fc> ;

: interner-ablauf
	{ a b c d }
	<h> !" Interner Ablauf" </h>
	<p>
		71 <->
		1 <|> !"  Beschreibung" 18 <|> s\"  ... <p> <i> !\" text \" </i> </p> ..." !! 56 <|> !"  in forth" 71 <|>
		71 <->
		1 <|> !"  Speicheraufbau" 18 <|> s\"  {p} 5 {i} {!!} addr len {/i} {/p} " !! 56 <|> s\"  here-\"stack\"" !! 71 <|>
		<br>
		1 <|> <b> a b </b> !! 18 <|> <b> c d !! </b> 56 <|> 71 <|>
		71 <->
	</p>
;

<presentation>
	<p>
		<file> s" header.txt" 0 100 </file>
		<br>
		\ <b> !" Denis Knauf & Harald Steinlechner" </b>
	</p>
<np>
	<h> !" Präsentationssoftware in Forth" </h>
	<p>
		<br>
		<br>
		!" Die flexible Forth-Syntax erlaubt die deklarative Representation von formatierten Text in Forth."
		<br>
		!" Die Präsentation selbst sowie ihre Seiten werden mittels <html> artigen Tags implementiert." 
	</p> 
<np>
	<h> !" Grundstruktur " </h>
	<p> <source> s" example.p.fs" 0 100 </source> </p>
<np>
	<h> !" Grundstruktur " </h>
	<p> <source> s" example.p.fs" 0 100 </source> </p>
	<h> !" Ergebnis" </h>
	<h> !" Dies ist eine Testpraesentation!" </h>
	<p>
		!" Eines Tages hatten wir [Harald Steinlechner und Denis Knauf"
		!" ] die tolle Idee, eine Praesentationssoftware zu schreiben."
	</p>
( <np>
	<h> !" Mit den wichtigsten Wörtern:" </h>
	<p>
		<li> <f> !" <presentation>" </f> !" : damit beginnt die praesentation" </li>
		<li> <f> !" <h>" </f> !" : Eine Ueberschrift" </li>
		<li> <f> !" <p>" </f> !" : Ein Paragraph" </li>
		<li> <f> !" <b>" </f> !" : Fettdruck" </li>
		<li> <f> !" <br>" </f> !" : Zeilenumbruch" </li>
		<li> <f> !\" !\"" </f> !" : Ein String" </li>
	</p> )
<np>
	<h> !" Benutzer Interaktion" </h>
	<p>
		!" Die Präsentation liegt im Speicher"
	</p>
	<p>
		!" ==> Dadurch wird die Navigation über Forth Wörter möglich."
	</p>
	<li> !" n   => nächste Seite" </li>
	<li> !" p   => vorige Seite"  </li>
	<li> !" u   => seite aktualisieren"  </li>
	<li> !" 3 g => Zur dritten Seite springen"  </li>
	<li> !" 3 n => 3 Seiten vor springen" </li>
<np> p4-1
<np> p4-1 p4-2
<np> p4-1 p4-2 p4-3
<np> p4-1 p4-2 p4-3
	<p> !" Eine neue Seite definieren: " <f> !" <np>" </f> </li>
<np>
	<h> !" Und Farben" </h>
	<br>
	<li> !" Hintergrundfarbe: " <f> s\" Yellow <bc> !\" text\" </bc> " !! </f> !" : " Yellow <bc> !\" text" </bc> </li>
	<li> !" Vordergrundfarbe: " <f> s\" Brown <fc> !\" text\" </fc> " !! </f> !" : " Brown <fc> !\" text" </fc> </li>
	<p> !" 8 Farben sind moeglich:" </p>
	<p> farbendemo </p>

<np>
	<h> !" Interner Aufbau" </h>
	<p>
		71 <->
		1 <|> <b> !"  Beschreibung" </b> 18 <|> 56 <|> <b> !"  in forth" </b> 71 <|>
		71 <->
		1 <|> 18 <|> 56 <|> 71 <|>
		71 <->
	</p>
<np>
	<h> !" Interner Aufbau" </h>
	<p>
		71 <->
		1 <|> !"  Beschreibung" 18 <|> 56 <|> !"  in forth" 71 <|>
		71 <->
		1 <|> <b> !"  Speicheraufbau" </b> 18 <|> 56 <|> <b> !\"  here-\"stack\"" </b> 71 <|>
		71 <->
	</p>
<np>
	<h> !" Interner Aufbau" </h>
	<p>
		71 <->
		1 <|> !"  Beschreibung" 18 <|> <b> s\"  ... <p> <i> !\" text \" </i> </p> ..." !! </b> 56 <|> !"  in forth" 71 <|>
		71 <->
		1 <|> !"  Speicheraufbau" 18 <|> 56 <|> !\"  here-\"stack\"" 71 <|>
		71 <->
	</p>
<np>
	<h> !" Interner Aufbau" </h>
	<p>
		71 <->
		1 <|> !"  Beschreibung" 18 <|> s\"  ... <p> <i> !\" text \" </i> </p> ..." !! 56 <|> !"  in forth" 71 <|>
		71 <->
		1 <|> !"  Speicheraufbau" 18 <|> <b> s\"  {p} 5 {i} {!!} addr len {/i} {/p} " !! </b> 56 <|> !\"  here-\"stack\"" 71 <|>
		71 <->
	</p>

<np> s"  Execute" s"   ^" interner-ablauf
<np> s"  {p}"     s"      ^" interner-ablauf
<np> s"  Execute" s"         ^" interner-ablauf
<np> s"  Execute" s"             ^" interner-ablauf
<np> s"  {!!}"    s"                   ^---^" interner-ablauf
<np> s"  Execute" s"                            ^" interner-ablauf
<np> s"  Execute" s"                                 ^" interner-ablauf

<np>
	<p>
		<h> !" Zeit für Makros!!" </h>
		<source> s" presentation.p.fs" 29 39 </source>
		<p> farbendemo </p>
	</p>
<np>
	<p>
		<h> !" Beliebige Wörter können Inhalte erzeugen!!" </h>
		<br> <br>
		<b> !" So kann man Aufzaehlungen erstellen" </b>
		<br> <br>
		<en>
			<||> !" Das erstellen von Präsentationen und Formatierungen wirkt natürlich" </||>
			<||> !" Unsere Sprache erbt die gesamte Funktionalität von Forth persönlich. " </||>
			<||> !" Makros generieren Inhalte" </||>
		</en>
		<source> s" presentation.p.fs" 112 116 </source>
		<br> !" Die Operatoren sehen so aus:" <br>
		<source> s" presentation.fs" 214 221 </source>
	</p>
<np>	
	<p> <tw>
		<h> !" Spezielle (verwendete) Features von Forth" </h>
		<br> <br>
		  <li> !" * Compiler VS Interpreter" </li>
		  <li> !" * Execution Tokens" </li>
		  <li> !" * Here , ," </li>
		  <li> !" * Makros" </li>
	</tw> </p>
<np>
	<p>
		<file> s" nochFragen.txt" 0 100 </file>
		<tw>
		<b> !" denis.knauf@gmail.com | haraldsteinlechner@gmail.com" </b> 
		</tw>
	</p>

</presentation>
