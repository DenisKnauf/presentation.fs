#! /usr/bin/gforth

\ variable cr-count
\ : cr cr cr-count dup @ 1+ swap ! ;
\ : cr-reset 0 cr-count ! ;
\ : page page cr-reset ;

\ 2-rot ist bei gforth anscheinend nicht definiert
: 2-rot { a1 a2 b1 b2 c1 c2 } c1 c2 a1 a2 b1 b2 ;

needs ansi.fs

\ at-xy? war mal bestandteil von gforth:
\ http://www.complang.tuwien.ac.at/cvsweb/cgi-bin/cvsweb/gforth/contrib/ansi.fs?rev=1.1;hideattic=0
: read-cdnumber  ( c - n | read a numeric entry delimited by character c)
	>r 0 begin
		key dup r@ -
		while
		swap 10 * swap [char] 0 - +
	repeat
	r> 2drop
;
: at-xy?  ( -- x y | return the current cursor coordinates)
	ESC[ ." 6n"
	key drop key drop  \ <esc> [
	[char] ; read-cdnumber [char] R read-cdnumber
	1- swap 1-
;

: escape ( -- addr len ) s\" \e" ;
: csi ( -- addr len ) s\" \e[" ;
: sgr ( u -- ) ESC[ 0 .r [char] m emit ;
: term-size form ;
: term-height ( -- i ) term-size drop ;
: term-width ( -- i ) term-size nip ;
: cursor@ ( -- x y ) at-xy? ;
: cursor! ( x y -- ) at-xy ;
: cursorx@ ( -- x ) cursor@ drop ;
: cursory@ ( -- y ) cursor@ nip ;
: cursorx! ( x -- ) cursory@ cursor! ;
: cursory! ( x -- ) cursorx@ swap cursor! ;

: cursor' ( i c -- ) swap dup if ESC[ 0 u.r emit else 2drop then ;
: cursor^ ( i -- ) [char] A cursor' ;
: cursor_ ( i -- ) [char] B cursor' ;
: cursor> ( i -- ) [char] C cursor' ;
: cursor< ( i -- ) [char] D cursor' ;
: screen^ ( i -- ) [char] S cursor' ;
: screen_ ( i -- ) [char] T cursor' ;

: isnewline? ( c -- i ) dup 10 = swap 13 = or ;
: isspace?   ( c -- i ) dup  9 = over 11 = or swap 32 = or ;
: anyspaces? ( c -- i ) dup isnewline? isspace? or ;

: clearwspace ( c -- c )
	dup
	case
	9  of 32 endof
	11 of 32 endof
	13 of 10 endof
		dup
	endcase nip
;

variable typewriter-delay
: typewriter-type ( addr len -- )
	typewriter-delay @ -rot
	over + swap +do
		dup ms i @ emit
	loop
	drop
;
50 typewriter-delay !

variable scroll-delay
: scroll-type ( addr len -- )
	dup 1 < if 2drop exit then
	term-width over - dup cursorx@ - swap cursorx!
	-rot 2dup type scroll-delay @ ms dup 1 + cursor< 2dup type s"   " type
	rot scroll-delay @ swap 1 +do
		dup ms -rot dup 2 + cursor< 2dup type s"  " type rot
	loop
	2drop drop
;
10 scroll-delay !

variable ptype-indent \ Zeile Einruecken
variable ptype-curx \ cursorx@-emulation durch zaehlen.
: ptype-cursorx@ ( -- i ) ptype-curx @ ;
: ptype-curx@! ( -- ) cursorx@ ptype-curx ! ;
defer ptype-type
' type is ptype-type
: ptype-word ( addrw addrc c -- addrc+1 )
	-rot \ c addrw addrc
	dup -rot over - ptype-type \ c addrc
	swap emit 1+ \ addrc+1
;
: ptype-init ( addr len lenm x -- addre lenm lenl addrw addre addr )
	dup ptype-indent @ \ addr len lenm x x indent
	swap - \ addr len lenm x indent-x
	dup 0>= if
		cursor> drop ptype-indent @
	else drop then
	{ addr len lenm x }  addr len +  lenm  x  addr  addr len +  addr
;
: ptype-newline ( lenm lenl addrw addrc -- lenm lenl addrw )
	10 ptype-word \ lenm lenl addrw=addrc+1
	ptype-indent @ dup cursor> swap \ lenm lenl0 addrw
;
: ptype-space ( lenm lenl addrw addrc -- lenm lenl1 addrw )
	32 ptype-word \ lenm lenl addrw=addrc+1
	swap 1+ swap \ lenm lenl+=1 addrw
;
: ptype-anychar ( lenm lenl addrw addrc -- lenm lenl addrw addrc )
	2over <=
	if
		\ lenm lenl addrw addrc
		rot tuck over swap - \ lenm addrw lenl addrc addrc-lenl
		2over drop >= \ lenm addrw lenl addrc addrc-lenl>=addrw
		if \ Wort ist laenger als eine Zeile -> muss umgebrochen werden.
			1- -rot 1- -rot 2dup - \ lenm lenl addrc-1 addrw addrc-1-addrw
			ptype-type ." -" nip 1 swap dup \ lenm lenl addrw=addrc-1 addrc-1
		else \ Wort erst in der naechsten Zeile ausgeben.
			nip 2dup - negate -rot \ lenm addrc-addrw addrw addrc
		then
		cr
		rot ptype-indent @ dup cursor> + -rot
	then
	rot 1+ -rot
;
: ptype' ( addre lenm lenl addrw addre addr -- )
	\ addre ist fuer die schleife unwichtig
	+do \ lenm lenl addrw
		i dup c@ \ lenm lenl addrw addrc c
		clearwspace
		case \ lenm lenl addrw addrc c
		10 of ptype-newline endof
		32 of ptype-space endof
			drop ptype-anychar
		endcase
	loop \ addre lenm lenl addrw
	swap ptype-curx ! nip tuck - ptype-type
;
: ptype ( addr len -- ) term-width 1- ptype-cursorx@ ptype-init ptype' ;
: ptype-reset ( -- )
	0 ptype-indent !
	0 ptype-curx !
; \ Nicht einruecken
ptype-reset

: beep 0 term-height 2 - cursor! 7 emit s" *beep* not" type ;

\ Es folgen ein paar syntaktische Textauszeichnungen.
: {i}   ( addr -- addr )  7 sgr ;
: <i>   ( -- ) ['] {i}  , ;
: {/i}  ( addr -- addr ) 27 sgr ;
: </i>  ( -- ) ['] {/i} , ;
: {b}   ( addr -- addr )  1 sgr ; \ bold
: <b>   ( -- ) ['] {b}  , ;
: {/b}  ( addr -- addr ) 22 sgr ;
: </b>  ( -- ) ['] {/b} , ;
: {u}   ( addr -- addr )  4 sgr ; \ underline
: <u>   ( -- ) ['] {u}  , ;
: {/u}  ( addr -- addr ) 24 sgr ;
: </u>  ( -- ) ['] {/u} , ;
: {fc}  ( addr -- addr ) dup @ 30 + sgr cell+ ; \ frontcolor
: <fc>  ( -- ) ['] {fc} , , ;
: {/fc} ( addr -- addr ) 39 sgr ;
: </fc> ( -- ) ['] {/fc} , ;
: {bc}  ( addr -- addr ) dup @ 40 + sgr cell+ ; \ backgroundcolor
: <bc>  ( -- ) ['] {bc} , , ;
: {/bc} ( addr -- addr ) 49 sgr ;
: </bc> ( -- ) ['] {/bc} , ;
: {br}  ( addr -- addr ) cr ptype-reset ;
: <br>  ( -- , xt-{br} ) ['] {br} , ;
: {animation}  ( addr , xt -- addr ) dup @ ['] ptype-type defer! cell+ ;
: <animation>  ( xt -- addr u- , xt-{animation} xt ) ['] {animation} , , ;
: {/animation} ( addr -- addr ) ['] type is ptype-type ;
: </animation> ( -- , xt-{/animation} ) ['] {/animation} , ;
: <tw>  ( -- , xt xt ) ['] typewriter-type <animation> ;
: </tw> ( -- , xt ) </animation> ;
: <scroll>  ( -- , xt xt ) ['] scroll-type <animation> ;
: </scroll> ( -- , xt ) </animation> ;
\ Es folgen ein paar blockorientierte Kennzeichnungen.
: {h}   ( addr , len -- addr )
	cr
	term-width over @ - 2 / \ addr width-twidth/2
	dup ptype-curx ! cursor>
	cell+ {b}
; \ header
: <h>   ( -- addr 0 , xt-{h} 0 )  ['] {h}  , here 0 , 0 ;
: {/h}  ( addr , len -- addr1 )
	{/b} cursorx@ 1+ \ addr x
	cr
	over @ term-width swap - 2 / 1- \ addr x indent
	cursor> \ addr x
	cursorx@ +do ." =" loop \ addr
	cr cell+
;
: </h>  ( addr len -- , xt-{/h} len0 ) ['] {/h} , dup , swap ! ;
: {p}   ( addr -- addr ) cr ptype-reset cell+ ; \ paragraph
: <p>   ( -- addr u0 , xt-{p} 0 ) ['] {p}  , here 0 , 0 ;
: {/p}  ( addr -- addr ) cr ;
: </p>  ( addr len -- , xt-{/p} ) ['] {/p} , swap ! ;
: {li}  ( addr -- addr )
	2 cursor> space 1 cursor>
	6 dup ptype-indent ! ptype-curx !
	cell+
;
: <li>  ( -- addr u0 , xt-{li} 0 ) ['] {li} , here 0 , 0 ;
: {/li} ( addr -- addr ) cr ;
: </li> ( addr len -- , xt-{/li} ) ['] {/li} , swap ! ;
33 constant table-color
: {|}   ( addr -- addr )
	dup @ dup ptype-cursorx@ - \ addr > >-i
	dup 1 < if
		drop
	else
		1- spaces
	endif
	ptype-curx ! table-color sgr ." |" 39 sgr cell+
;
: <|>   ( i -- addr , xt-{|} i ) ['] {|} , , ;
: {-}   ( addr -- addr ) cr dup @ 0 table-color sgr +do [char] - emit loop 39 sgr cell+ cr ;
: <->   ( i -- addr , xt-{-} i ) ['] {-} , , ;

variable enumerationCount ( -- addr )
: {||}  ( addr -- addr )                \ increments enumeration count and prints prefix
	enumerationCount dup @ 1+ dup rot !
	2 cursor> 0 u.r ." ." 1 cursor>
	6 dup ptype-indent ! ptype-curx !
;
: <||>  ( -- , xt-{||} 0 ) ['] {||} , ;
: {/||} ( addr -- addr ) cr ;
: </||> ( -- , xt-{/||} ) ['] {/||} , ;

: {en}  ( -- )
	5 ptype-indent ! \ 6 Zeichen weit einruecken
	0 enumerationCount ! \ resets enumeration count
;
: <en>  ( -- , xt-{en}  )  ['] {en} , ;
: {/en} ( -- ) ptype-reset ;
: </en> ( -- , xt-{/en} )  ['] {en} , ;

256 Constant max-line
Create line-buffer  max-line 2 + allot

0 Value fd-in
: open-input ( addr u -- )  r/o open-file throw to fd-in ;

: printsource ( from to addr u 1/0 -- )
	{ showLines }
	open-input
	cr
	0
	begin
		1+ line-buffer max-line fd-in read-line throw
	while
		swap 2over rot tuck >= if
			tuck <= if
				dup 0 <# #s #> \ ... i str l
				dup ptype-indent @ \ i str l l indent
				dup ptype-curx ! 1- \ i str l l indent
				swap - dup 0< if drop else cursor> then \ i str l
				showLines if
					type ." |" \ ... i  \ Eingerueckt Zahl ausgeben
				else 2drop \ Eingerueckt keine Zahl ausgeben
				endif
				swap line-buffer swap ptype cr
			else nip
			endif
		else nip nip
		endif
	repeat 
	2drop 2drop 
	fd-in close-file throw
;

: printCodeHeader ( end start namelen addr 1/0 -- )  \ prints source code header containing line numbers
  { showLines }
  swap 2swap \ addr namelen end start
	2dup > if swap then \ addr namelen start/end end/start
	showLines if dup 0 <# #s #> nip else 0 then ptype-reset 1+ ptype-indent !
  2swap \ start end addr namelen
	showLines printsource cr
;

: {source}  ( -- ) ;
: <source>  ( -- , xt-{source}  )  ['] {source} , ;
: {/source} ( -- ) dup dup dup dup @ swap cell + @  2swap cell 2 * +
		   @ swap cell 3 * + @ 1 printCodeHeader 4 cells + ;
: </source> ( -- , xt-{/source} )  ['] {/source} , , , , , ;

: {file}  ( -- ) ;
: <file>  ( -- , xt-{file}  )  ['] {file} , ;
: {/file} ( -- ) dup dup dup dup @ swap cell + @  2swap cell 2 * +
		   @ swap cell 3 * + @ 0 printCodeHeader 4 cells + ;
: </file> ( -- , xt-{/file} )  ['] {/file} , , , , , ;

: {np} ( -- )
	0 sgr \ Alle Bildschirmeigenschaften zuruecksetzen
	page \ Bildschirm leeren
;
: {/np} ( -- )
	\ 30 sgr 40 sgr
	0 term-height dup cursory@ - 2 / dup 0< if drop else screen_ then 2 - cursor!
;
: <np> ( -- addr , xt-{/np} xt-{np} )
	\ Wir legen jede Anfangsadresse einer Seite auf den Stack (Achtung, in umgekehrter Reihenfolge)
	['] {/np} ,
	here
	['] {np} ,
;

: {!!} ( addr -- addr+2 )
	dup @   \ addr straddr
	swap    \ straddr addr
	cell+   \ straddr addr
	tuck    \ addr straddr addr
	@       \ addr straddr strlen
	ptype   \ addr
	cell+
;
: !! ( len0 addr1 len1 -- len , xt-{!!} addr1 len1 )
	['] {!!} ,
	dup \ len0 addr1 len1 len1
	rot , , \ len0 len1 len1 addr1 -> len0 len1
	+ \ len0+len1
;
:noname ( -- ) 34 parse save-mem !! ; :noname 34 parse postpone sliteral postpone !! ; interpret/compile: !"
:noname ( -- ) \"-parse save-mem !! ; :noname \"-parse postpone sliteral postpone !! ; interpret/compile: !\"

: pres_page_cur ( addr -- addr ) ;
: pres_page_from ( addr -- addr ) cell+ ;
: pres_page_to ( addr -- addr ) 2 cells + ;

variable pres-restore 3 cells allot

\ faddr: erste Seitenzeigeradresse (letzte Presentationsseite)
\ laddr: letzte Seitenzeigeradresse (erste Presentationsseite)
\ paddr: derzeitige Seitenzeigeradresse

: page_steps ( laddr paddr 0 [u] -- u )
	\ u muss ungleich 0 sein. falls u nicht vorhanden: 1
	dup 0= if 1 then \ laddr paddr 0 u
	nip
;
: validpage? ( faddr laddr paddr -- faddr laddr paddr u )
	2dup <= \ faddr laddr paddr u
	2over drop rot tuck >
	rot \ faddr laddr paddr u u
	if drop cell - 1 \ faddr laddr paddr-1 -1 \ paddr-overflow
	else if cell+ -1 \ faddr laddr paddr+1 -1 \ paddr-underflow
	else 0 \ faddr laddr paddr 0
	then then
;
: showpage' ( paddr -- )
	dup cell - \ paddr paddr+cell
	@ swap @ \ naddr addr \ Seiteninhaltsadressen
	begin 2dup >
	while
		dup cell+ swap \ naddr xtaddr xtaddr
		@ \ naddr xtaddr xt
		execute \ verschiebt eventuell den Zeiger noch weiter, wenn es Parameter erwartet.
	repeat
	2drop
;
: showpage ( faddr laddr paddr -- faddr laddr paddr0 0 )
	validpage? 0 tuck 2-rot drop \ i 0 faddr laddr paddr0
	dup showpage' 0 2rot drop \ faddr laddr paddr0 0 i
	if beep then
	2over swap pres-restore !
	pres-restore cell+ !
	over pres-restore 2 cells + !
;
: n ( faddr laddr paddr 0 [u] -- faddr laddr paddr 0 ) page_steps cells - showpage ;
: g ( faddr laddr paddr 0  u  -- faddr laddr paddr 0 ) cells nip nip over swap - showpage ;
: p ( faddr laddr paddr 0 [u] -- faddr laddr paddr 0 ) page_steps cells + showpage ;
: u ( faddr laddr paddr X     -- faddr laddr paddr 0 ) drop showpage ;
: q bye ;
: r ( -- faddr laddr paddr 0 )
	pres-restore dup @
	swap cell+ dup @
	swap cell+ @
	0
;

: <presentation> ( -- addr0 0 addr1 , xt-{np} ) here 0 here ['] {np} , ;
: </presentation> ( 0 <addr...> -- faddr laddr paddr 0 !! endaddr 0 0 0 0 <...addr> )
	<np>
	here \ 0 <addr...> faddr
	begin swap dup \ 0 <addr..> addr0 faddr
	while , \ 0 <addr..> faddr
	repeat \ .s cr
	drop \ faddr
	here dup 0 \ faddr laddr paddr 0
	\ u \ Praesentation starten
;
