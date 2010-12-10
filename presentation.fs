#! /usr/bin/gforth
: copy ( addrdst addrsrc len -- addrdstend )
	over ( dst src len src ) + swap ( dst end src )
	do ( dst+ )
		i ( dst+ src+ ) c@ ( dst+ chr )
		over ( dst+ chr dst+ ) c! ( dst+ ) 1+
	loop
;

: </presentation> 0 ;
: page_steps ( 0 [x] -- 0 x )
	\ x muss ungleich 0 sein. falls x nicht vorhanden: 1
	dup 0= if 1 then
;
: n ( 0 [x] -- 0 )
	page_steps
	( ... x seiten weiterspringen ... )
;
: csi 27 c, 91 c,
: <--> ;
: <_> ;
: <h> <--> ;
: <b> csi c, <_> ;
: <i> csi c, <_> ;
: <np> begin , 0<> until ;
\ : <+> ( addr1 len1 addr2 len2 -- addrdst lendst )
\	rot 2dup + here ( addr1 addr2 len2 len1 lendst addrdst )
\	2-rot -rot ( lendst addrdst addr1 len1 addr2 len2 )
\	2swap 2rot ( addr2 len2 addr1 len1 lendst addrdst )
\	2dup chars allot ( dst allocated )
\	copy copy
\ ;
: @@ ( addr len -- )
	here -rot ( dst src len )
	copy drop
;

bye

<presentation>
s" Dies ist eine Testpresentation" <h>
s" Eines Tages hatten wir (" @@ <b> s" Harald Steinlechner" @@ </b> s" und" @@
	<b> s" Denis Knauf" @@ </b> s" die tolle Idee, eine Presentationssoftware zu schreiben" @@ <p>
<np>
s" Ergebnis:" @@ <h>
<b> s" Das hier" @@ </b> <p>
<np>
s" Sieht doch garnicht so schlecht aus" @@ <p>
</presentation>

\ presentation ist gestartet: erste Seite wird angezeigt
n \ zweite Seite
p \ erste Seite
2 n \ dritte Seite
