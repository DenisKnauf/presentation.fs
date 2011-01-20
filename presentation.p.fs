
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
		<li> !" <h>"   </li>
		<li> !" <p>"  </li>
		<li> !" <b>"  </li>
		<li> !" <br>" </li>
		<br>
		<li> !\" !\" " </li>
	</p>
<np>
	<p>
		<h> !" Memory layout von <prasentation> " </h>
		<br>
		<br>
		!" Sollten wir das hier beschreiben?"
	</p>
<np>
	<p>
		<h> !" Wie <prasentation> abgearbeitet wrid" </h>
	</p>
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
</presentation>
