# RendezvousNet
Rendezvous also known as highest random weight (HRW) hashing implementation.

# What is rendezvous hashing?

It's a non so well known alternative to well-known consistent hashing(even though rendezvous hashing precedes consistent for around a year).

# What is it good for?

As a consistent hashing algorithm it maintains K/n remapping invariant(K/n keys are remapped whenever the list of endpoints changes (K is the total number of keys and n is the total number of nodes/servers)).

# Is it better than consistent hashing?

Not really, the main problem with rendezvous hashing is that it's somehow hard to deal with the hotspot problem, should it arise.

# Why use rendezvous hashing then?

1. Lightweight memory consumption, the only thing that you need to store in a non-optimized version is the list of nodes.
2. It's easier to understand and implement.
3. Provides a good disxtribution without a lot of complexity(it also depends on the hash functioin that you use to hash (node,key) pair).
