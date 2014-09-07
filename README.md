RightloadUrlAutoReplace, a small command line application by Admiral H. Curtiss for auto-inserting uploaded images from Rightload into an LP update.
Feel free to modify and/or redistribute.

Usage:

    RightloadUrlAutoReplace [options] update.txt urls.txt [update_replaced.txt] [placeholder prefix] [placeholder postfix]
    
    Options:
     -keepextension         Keep filename extensions in placeholders, ie. search for [image1.png] instead of just [image1].

In short, this tool replaces placeholders for images in a text file with the actual [img]-tagged links to those images, which are provided in a separate file.

So running it on this:

update.txt:

    [tov57_ps3_ss003681]
    random text
    [tov59_ss029044]
    more text
    [tov59_ss030964]
    stuff here

urls.txt:

    [IMG]http://lpix.org/719958/tov57_ps3_ss003681.jpg[/IMG]
    [IMG]http://lpix.org/747200/tov59_ss029044.jpg[/IMG]
    [IMG]http://lpix.org/747201/tov59_ss030964.jpg[/IMG]

with the command line:

    RightloadUrlAutoReplace update.txt urls.txt

results in update_replaced.txt containing:

    [img]http://lpix.org/719958/tov57_ps3_ss003681.jpg[/img]
    random text
    [img]http://lpix.org/747200/tov59_ss029044.jpg[/img]
    more text
    [img]http://lpix.org/747201/tov59_ss030964.jpg[/img]
    stuff here


More notes:
- [img] tags are automatically identified and removed from the urls.txt file, but if they're not there that's fine too.
- If you don't want to use the default [filename] with brackets and instead use, for example, (filename) with round braces as your placeholders, you can give those as pre- and postfixes to the program with additional arguments, as in:
    RightloadUrlAutoReplace update.txt urls.txt update_new.txt ( )
