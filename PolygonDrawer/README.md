# Polygon drawer
### User manual
A textbox providing relevant instructions is displayed within the application.

You can drag either a point, a Bezier line control point, or the whole polygon by holding LMB and moving it. Right-clicking a point or a line
opens up a context menu with actions regarding the inspected element. You can enter polygon creation mode by clicking
the "New polygon" button (the creation will finish after you've connected back to the starting point). The app contains a button
for toggling captions, and a radio button for switching a line drawing mechanism.

### Implemented relations
The primary classes used in the project are Polygon, Line and Point. Polygon keeps its lines and points in lists inside of itself,
while points keep references to their neighboring lines, meanwhile lines keep information about their adjacent points.
This is important, because the relations only need to be managed when adding/removing points, but it eases the access
to desired elements from any point of the application. Moreover it helps handling some special cases (for example,
Bezier curve control segments are not listed as lines inside of the polygon, they are, however, marked as the next
lines from the points preceding the curve, which helps with mantaining desired continuity.

Whenever a point is moved, it sends a signal (mimicking a recursion) to adjacent points, which adjusts them
according to constraints of the connecting line/continuity of a vertex between them. Those points send the
signal further after being moved. To prevent infinite recursion, in case all of the edges are somehow restricted,
the signal isn't being sent back to the vertex it came from, nor will it affect the point from which it originally started.

If a starting/ending point of a Bezier curve is moved, the control points adjust themselves, trying to preserve the
shape of the curve (if, for example both of the ending points have G0 continuity set, the shape should be preserved
perfectly, outside of some edge cases, otherwise the shape is mostly preserved, but other continuities may cause other movements,
that modify the shape slightly.
