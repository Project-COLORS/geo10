so i don't forget why

## class variable prefixes
- _name: a normal member that contains a value that has significance, also public externally set members.
- t_name: temp member. member made for express purpose of speeding up something. don't expect variable to have significance at all points in time
- c_name: "const" or "readonly" value. a variable for purposes of setting numbers that are reused often, but shouldn't be set during operation.

## retired prefixes
- r_name: public reference member. reserved for public references to other objects that need to be set via the editor UI. also more specifically reserved for external objects that are used *actively* as opposed to things like prefabs.
- pr_name: prefab reference. a non-active externally set reference