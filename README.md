# ParentRepeaterLayout
A simple-as-possible set of page layout rules that allows for a wide range of content- and display-aware layout

## Concept

Outer most container is fixed size.
There is a single chain/list (NOT tree) of sub-container types for the document.
Each type has two link points (next,prev), which are relative to one of the corners (TL,TR,BL,BR).
Each type has either a fixed or relative horizontal and vertical size.
Zero size means it will grow to fit its children, as long as that doesn't overflow the parent.


Contents are added to the leaf type, giving a fixed size (like the image size or glyph size).
The layout is filled by adding a list of contents.
This list can also contain 'breaks' which ends containers up to a certain level (like BRK1 stops the word, BRK2 stops the line, BRK3 stops the column)

When a content item can't fit in its container, a new parent container is spawned, and joined next->prev.
If that container can't fit, the next parent up is created the same way.
This continues up until success. If no success in the entire chain, the document is full.
If a newly created container can't hold a content item, is this a fail?

If the type has glue set, then the content will break it's parent container if not all items can be fit together.
This is useful for keeping letters together in words, but requires you to manually break each run


This is an expansion of the sketch at https://jsfiddle.net/i_e_b/49zhr13b/
