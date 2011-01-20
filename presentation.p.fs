
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
	<p> !" Eine neue Seite definieren: " <f> !" <np>" </f> </li>
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

<presentation>
	<p>
		<file> s" header.txt" 0 100 </file>
		<br>
		\ <b> !" Denis Knauf & Harald Steinlechner" </b>
	</p>
<np>
	<p>
		<h> !" Präsentationssoftware in Forth" </h>
		<br>
		<br>
		!" Die flexible Forth-Syntax erlaubt die deklarative Representation von formatierten Text in Forth."
		<br>
		!" Die Präsentation selbst sowie ihre Seiten werden mittels <html> artigen Tags implementiert."
	</p>
<np>
	<p>
		<h> !" Grundstruktur " </h>
		<br>
		<br>
		<source> s" example.p.fs" 0 100 </source>
	</p>
<np>
	<p>
		<h> !" Die wichtigsten Wörter" </h>
		<br>
		<br>
		<li> !" <prasentation>" </li>
		<li> !" <p>" </li>
		<li> !" <b>" </li>
		<li> !" <br>" </li>
	</p>
<np>
	<h> </b> !" Dies ist eine Testpraesentation!" </h>
	<p>
		!" Eines Tages hatten wir [Harald Steinlechner und Denis Knauf"
		!" ] die tolle Idee, eine Praesentationssoftware zu schreiben."
		!" text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text"
	</p>
	\ <p> <en>
		\ <||> !" Forth eignet sich gut für Domain specific languages." </||> 
		\ <||> !" Makros sind ein Heidenspass." </||> 
		\ <||> !" Execution tokens erzeugen unseren Code. text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text text" </||> 
	\ </en> </p>
<np>
	<h> !" Ergebnis" </h>
	<p> <b> !" Das hier" </b> </p>
<np>
	<h> !" hallo" </h>
	<p> !" Sieht doch garnicht so schlecht aus" </p>
<np>
	p4-1
<np>
	p4-1 p4-2
<np>
	p4-1 p4-2 p4-3
<np>
	<h> !" Und Farben" </h>
	<br>
	<li> !" Hintergrundfarbe: " <f> s\" Yellow <bc> !\" text\" </bc> " !! </f> !" : " Yellow <bc> !\" text" </bc> </li>
	<li> !" Vordergrundfarbe: " <f> s\" Brown <fc> !\" text\" </fc> " !! </f> !" : " Brown <fc> !\" text" </fc> </li>
	<p> !" 8 Farben sind moeglich:" </p>
	<p> farbendemo </p>
<np>
	<h> !" Und Source" </h>
	<br>
	<source> s" presentation.fs" 0 20 </source>
</presentation>
