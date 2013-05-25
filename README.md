RightloadUrlAutoReplace, a small command line application by Admiral H. Curtiss for auto-inserting uploaded images from Rightload into an LP update.
Feel free to modify and/or redistribute.

Usage:

    RightloadUrlAutoReplace update.txt urls.txt [update_replaced.txt] [replace name prefix] [replace name postfix] [-keepextension]

Essentially, what it does is, for each URL in urls.txt, update.txt is searched for the combination of prefix+filename-without-extension+postfix and, if found, replaced by the URL from urls.txt. So running it on this:

update.txt:

    [tov57_ps3_ss003681]
    random text
    [tov59_ss029044]
    more text
    [tov59_ss030964]
    stuff here
    [tov59_ss031564]
    and more

urls.txt:

    [IMG]http://lpix.org/719958/tov57_ps3_ss003681.jpg[/IMG]
    [IMG]http://lpix.org/747200/tov59_ss029044.jpg[/IMG]
    [IMG]http://lpix.org/747201/tov59_ss030964.jpg[/IMG]
    [IMG]http://lpix.org/747202/tov59_ss031564.jpg[/IMG]

with the command line:

    RightloadUrlAutoReplace update.txt urls.txt

results in:

    [img]http://lpix.org/719958/tov57_ps3_ss003681.jpg[/img]
    random text
    [img]http://lpix.org/747200/tov59_ss029044.jpg[/img]
    more text
    [img]http://lpix.org/747201/tov59_ss030964.jpg[/img]
    stuff here
    [img]http://lpix.org/747202/tov59_ss031564.jpg[/img]
    and more


More notes:
- [img] tags are automatically identified and removed from the urls.txt file, but if they're not there that's fine too.
- If you don't want to use [filename] and instead use, for example, (filename) as your placeholders, you can give those pre- and postfixes to the program as additional arguments, as in:
  RightloadUrlAutoReplace update.txt urls.txt update_new.txt ( )
- Specifying the -keepextension flag at the end of the arguments keeps the .jpg/.png/.gif extension at the end of the URL when searching for pattern matching. So if you use [image.png] as your placeholder rather than just [image], use:
  RightloadUrlAutoReplace update.txt urls.txt update_new.txt [ ] -keepextension
  (This was implemented because someone used both image001.gif and image001.png within the same update, when they are in fact not the same image.)
