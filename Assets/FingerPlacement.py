import math


# finds the distance between two inputted points
def distance(x1, y1, x2, y2):
    return math.hypot((x2 - x1), (y2 - y1))


class FingerPlacement:
    def __init__(self, cr):
        self.circle_radius = cr

    def set_radius(self, cr):
        self.circle_radius = cr

    # finds which fingers are up and which are down
    def get_fingers(self, circle_positions, lm_positions):
        placement = ""

        for i in range(len(circle_positions)):
            # Finds current distance between the center of the circle and the fingertip
            mc_index = i*4+3
            fin_index = i*4+4
            dis = distance(lm_positions[fin_index][0], lm_positions[fin_index][1],
                           circle_positions[i][0]+lm_positions[0][0], circle_positions[i][1]+lm_positions[0][1])

            # if fingertip is outside the circle, then it is considered "down" (True)
            if dis <= self.circle_radius:
                placement += "T"
            else: # if fingertip is inside the circle, then it is considered "up" (False)
                placement += "F"

        return placement
