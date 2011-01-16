
\ Haeufig benutzte Textauszeichnungen:
: <f> <b> 4 <fc> ;
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
	<h> !" Dies ist eine Testpraesentation!" </h>
	<p>
		!" Eines Tages hatten wir [" <b> !" Harald Steinlechner" </b>
		!"  und " <b> !" Denis Knauf" </b>
		!" ] die tolle Idee, eine Praesentationssoftware zu schreiben."
		<br>
		<br>	
		<b>	
		<en>
                <||> !" erstens"  </||> 
		<||> !" zweitens" </||> 
		<||> !" dann"     </||> 
		</en>
		</b>
	</p>
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
	<li> !" Hintergrundfarbe: " <f> s\" 3 <bc> !\" text\" </bc> " !! </f> !" : " 3 <bc> !\" text" </bc> </li>
	<li> !" Vordergrundfarbe: " <f> s\" 5 <fc> !\" text\" </fc> " !! </f> !" : " 5 <fc> !\" text" </fc> </li>
	<p> !" 8 Farben sind moeglich:" </p>
	<p> farbendemo </p>
</presentation>
