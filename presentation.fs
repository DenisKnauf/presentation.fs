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
: csi 27 91 ;
: <h> ( -- addr 0 ) 2 c, here 0 ;
: </h> ( addr len -- ) 3 c, swap ! ;
: <p> ( -- addr 0 ) 4 c, here 0 ;
: </p> ( addr len -- ) 5 c, swap ! ;
: <i> ( -- ) 6 c, ;
: </i> ( -- ) 7 c, ;
: <b> ( -- ) 8 c, ;
: </b> ( -- ) 9 c, ;
\ : <np> begin , 0<> until ;
\ : <+> ( addr1 len1 addr2 len2 -- addrdst lendst )
\	rot 2dup + here ( addr1 addr2 len2 len1 lendst addrdst )
\	2-rot -rot ( lendst addrdst addr1 len1 addr2 len2 )
\	2swap 2rot ( addr2 len2 addr1 len1 lendst addrdst )
\	2dup chars allot ( dst allocated )
\	copy copy
\ ;
: !! ( len addr len -- len ) 1 c, dup rot , , + ;

bye

<presentation>
<h> s" Dies ist eine Testpresentation" !! </h>
<p>
	s" Eines Tages hatten wir (" !! <i> s" Harald Steinlechner" !! </i>
	s" und" !! <i> s" Denis Knauf" !! </i>
	s" die tolle Idee, eine Presentationssoftware zu schreiben" !!
</p>
<np>
<h> s" Ergebnis:" !! </h>
<p> <b> s" Das hier" !! </b> </p>
<np>
<p> s" Sieht doch garnicht so schlecht aus" !! </p>
</presentation>

\ presentation ist gestartet: erste Seite wird angezeigt
n \ zweite Seite
p \ erste Seite
2 n \ dritte Seite
